// ZipStorer, by Jaime Olivares
// Website: http://github.com/jaime-olivares/zipstorer
// Version: 3.4.0 (August 4, 2017)

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System.IO.Compression
{
    /// <summary>
    ///     Unique class for compression/decompression file. Represents a Zip file.
    /// </summary>
    public class Unzip : IDisposable
    {
        /// <summary>
        ///     Compression method enumeration
        /// </summary>
        public enum Compression : ushort
        {
            /// <summary>Uncompressed storage</summary>
            Store = 0,

            /// <summary>Deflate compression method</summary>
            Deflate = 8
        }

        /// <summary>
        ///     Represents an entry in Zip file directory
        /// </summary>
        public struct ZipFileEntry
        {
            /// <summary>Compression method</summary>
            public Compression Method;

            /// <summary>Full path and filename as stored in Zip</summary>
            public string FilenameInZip;

            /// <summary>Original file size</summary>
            public uint FileSize;

            /// <summary>Compressed file size</summary>
            public uint CompressedSize;

            /// <summary>Offset of header information inside Zip storage</summary>
            public uint HeaderOffset;

            /// <summary>Offset of file inside Zip storage</summary>
            public uint FileOffset;

            /// <summary>Size of header information</summary>
            public uint HeaderSize;

            /// <summary>32-bit checksum of entire file</summary>
            public uint Crc32;

            /// <summary>Last modification time of file</summary>
            public DateTime ModifyTime;

            /// <summary>User comment for file</summary>
            public string Comment;

            /// <summary>Overriden method</summary>
            /// <returns>Filename in Zip</returns>
            public override string ToString()
            {
                return FilenameInZip;
            }
        }
        
        #region Private fields

        // Stream object of storage file
        private Stream _zipFileStream;

        // Central dir image
        private byte[] _centralDirImage;

        // leave the stream open after the ZipStorer object is disposed
        private bool _leaveOpen;

        // Static CRC32 Table

        // Default filename encoder
        private static readonly Encoding DefaultEncoding = Encoding.GetEncoding(437);

        #endregion

        #region Public methods
        
        /// <summary>
        ///     Method to open an existing storage file
        /// </summary>
        /// <param name="filename">Full path of Zip file to open</param>
        /// <param name="access">File access mode as used in FileStream constructor</param>
        /// <returns>A valid ZipStorer object</returns>
        public static Unzip Open(string filename, FileAccess access)
        {
            var stream = new FileStream(filename, FileMode.Open,
                access == FileAccess.Read ? FileAccess.Read : FileAccess.ReadWrite);

            var zip = Open(stream, access);

            return zip;
        }

        /// <summary>
        ///     Method to open an existing storage from stream
        /// </summary>
        /// <param name="stream">Already opened stream with zip contents</param>
        /// <param name="access">File access mode for stream operations</param>
        /// <param name="leaveOpen">
        ///     true to leave the stream open after the ZipStorer object is disposed; otherwise, false
        ///     (default).
        /// </param>
        /// <returns>A valid ZipStorer object</returns>
        public static Unzip Open(Stream stream, FileAccess access, bool leaveOpen = false)
        {
            if (!stream.CanSeek && access != FileAccess.Read)
                throw new InvalidOperationException("Stream cannot seek");

            var zip = new Unzip
            {
                _zipFileStream = stream,
                _leaveOpen = leaveOpen
            };

            if (zip.ReadFileInfo())
                return zip;

            throw new InvalidDataException();
        }

        /// <summary>
        ///     Updates central directory (if pertinent) and close the Zip storage
        /// </summary>
        /// <remarks>This is a required step, unless automatic dispose is used</remarks>
        public void Close()
        {
            if (_zipFileStream != null && !_leaveOpen)
            {
                _zipFileStream.Flush();
                _zipFileStream.Dispose();
                _zipFileStream = null;
            }
        }

        /// <summary>
        ///     Read all the file records in the central directory
        /// </summary>
        /// <returns>List of all entries in directory</returns>
        public List<ZipFileEntry> ReadCentralDir()
        {
            if (_centralDirImage == null)
                throw new InvalidOperationException("Central directory currently does not exist");

            var result = new List<ZipFileEntry>();

            for (var pointer = 0; pointer < _centralDirImage.Length;)
            {
                var signature = BitConverter.ToUInt32(_centralDirImage, pointer);

                if (signature != 0x02014b50)
                    break;

                var encodeUTF8 = (BitConverter.ToUInt16(_centralDirImage, pointer + 8) & 0x0800) != 0;
                var method = BitConverter.ToUInt16(_centralDirImage, pointer + 10);
                var modifyTime = BitConverter.ToUInt32(_centralDirImage, pointer + 12);
                var crc32 = BitConverter.ToUInt32(_centralDirImage, pointer + 16);
                var comprSize = BitConverter.ToUInt32(_centralDirImage, pointer + 20);
                var fileSize = BitConverter.ToUInt32(_centralDirImage, pointer + 24);
                var filenameSize = BitConverter.ToUInt16(_centralDirImage, pointer + 28);
                var extraSize = BitConverter.ToUInt16(_centralDirImage, pointer + 30);
                var commentSize = BitConverter.ToUInt16(_centralDirImage, pointer + 32);
                var headerOffset = BitConverter.ToUInt32(_centralDirImage, pointer + 42);
                var headerSize = (uint) (46 + filenameSize + extraSize + commentSize);

                var encoder = encodeUTF8 ? Encoding.UTF8 : DefaultEncoding;

                var zfe = new ZipFileEntry
                {
                    Method = (Compression) method,
                    FilenameInZip = encoder.GetString(_centralDirImage, pointer + 46, filenameSize),
                    FileOffset = GetFileOffset(headerOffset),
                    FileSize = fileSize,
                    CompressedSize = comprSize,
                    HeaderOffset = headerOffset,
                    HeaderSize = headerSize,
                    Crc32 = crc32,
                    ModifyTime = DosTimeToDateTime(modifyTime) ?? DateTime.Now
                };

                if (commentSize > 0)
                    zfe.Comment = encoder.GetString(_centralDirImage, pointer + 46 + filenameSize + extraSize,
                        commentSize);

                result.Add(zfe);
                pointer += 46 + filenameSize + extraSize + commentSize;
            }

            return result;
        }

        /// <summary>
        ///     Copy the contents of a stored file into a physical file
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="filename">Name of file to store uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique compression methods are Store and Deflate</remarks>
        public bool ExtractFile(ZipFileEntry zfe, string filename)
        {
            // Make sure the parent directory exist
            var path = Path.GetDirectoryName(filename);

            if (!Directory.Exists(path))
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(path);

            // Check it is directory. If so, do nothing
            if (Directory.Exists(filename))
                return true;

            bool result;
            using (var output = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                result = ExtractFile(zfe, output);
            }

            if (result)
            {
                File.SetCreationTime(filename, zfe.ModifyTime);
                File.SetLastWriteTime(filename, zfe.ModifyTime);
            }

            return result;
        }

        /// <summary>
        ///     Copy the contents of a stored file into an opened stream
        /// </summary>
        /// <param name="zfe">Entry information of file to extract</param>
        /// <param name="stream">Stream to store the uncompressed data</param>
        /// <returns>True if success, false if not.</returns>
        /// <remarks>Unique compression methods are Store and Deflate</remarks>
        public bool ExtractFile(ZipFileEntry zfe, Stream stream)
        {
            if (!stream.CanWrite)
                throw new InvalidOperationException("Stream cannot be written");

            // check signature
            var signature = new byte[4];
            _zipFileStream.Seek(zfe.HeaderOffset, SeekOrigin.Begin);
            _zipFileStream.Read(signature, 0, 4);
            if (BitConverter.ToUInt32(signature, 0) != 0x04034b50)
                return false;

            // Select input stream for inflating or just reading
            Stream inStream;
            if (zfe.Method == Compression.Store)
                inStream = _zipFileStream;
            else if (zfe.Method == Compression.Deflate)
                inStream = new DeflateStream(_zipFileStream, CompressionMode.Decompress, true);
            else
                return false;

            // Buffered copy
            var buffer = new byte[16384];

            _zipFileStream.Seek(zfe.FileOffset, SeekOrigin.Begin);

            var bytesPending = zfe.FileSize;

            while (bytesPending > 0)
            {
                var bytesRead = inStream.Read(buffer, 0, (int) Math.Min(bytesPending, buffer.Length));
                stream.Write(buffer, 0, bytesRead);
                bytesPending -= (uint) bytesRead;
            }

            stream.Flush();

            if (zfe.Method == Compression.Deflate)
                inStream.Dispose();
            return true;
        }

        #endregion

        #region Private methods

        // Calculate the file offset by reading the corresponding local header
        private uint GetFileOffset(uint headerOffset)
        {
            var buffer = new byte[2];

            _zipFileStream.Seek(headerOffset + 26, SeekOrigin.Begin);
            _zipFileStream.Read(buffer, 0, 2);
            var filenameSize = BitConverter.ToUInt16(buffer, 0);
            _zipFileStream.Read(buffer, 0, 2);
            var extraSize = BitConverter.ToUInt16(buffer, 0);

            return (uint) (30 + filenameSize + extraSize + headerOffset);
        }

        private DateTime? DosTimeToDateTime(uint dt)
        {
            var year = (int) (dt >> 25) + 1980;
            var month = (int) (dt >> 21) & 15;
            var day = (int) (dt >> 16) & 31;
            var hours = (int) (dt >> 11) & 31;
            var minutes = (int) (dt >> 5) & 63;
            var seconds = (int) (dt & 31) * 2;

            if (month == 0 || day == 0)
                return null;

            return new DateTime(year, month, day, hours, minutes, seconds);
        }

        // Reads the end-of-central-directory record
        private bool ReadFileInfo()
        {
            if (_zipFileStream.Length < 22)
                return false;

            try
            {
                _zipFileStream.Seek(-17, SeekOrigin.End);
                var br = new BinaryReader(_zipFileStream);
                do
                {
                    _zipFileStream.Seek(-5, SeekOrigin.Current);

                    var sig = br.ReadUInt32();

                    if (sig == 0x06054b50)
                    {
                        _zipFileStream.Seek(6, SeekOrigin.Current);

                        br.ReadUInt16();
                        var centralSize = br.ReadInt32();
                        var centralDirOffset = br.ReadUInt32();
                        var commentSize = br.ReadUInt16();

                        // check if comment field is the very last data in file
                        if (_zipFileStream.Position + commentSize != _zipFileStream.Length)
                            return false;

                        // Copy entire central directory to a memory buffer
                        _centralDirImage = new byte[centralSize];
                        _zipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);
                        _zipFileStream.Read(_centralDirImage, 0, centralSize);

                        // Leave the pointer at the begining of central dir, to append new files
                        _zipFileStream.Seek(centralDirOffset, SeekOrigin.Begin);
                        return true;
                    }
                } while (_zipFileStream.Position > 0);
            }
            catch (Exception exception)
            {
                Debug.Fail(exception.Message, exception.ToString());
            }

            return false;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///     Closes the Zip file stream
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}