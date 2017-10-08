using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;

namespace Xml2WinForms.Utils
{
    public sealed class IconHolder
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IconHolder));

        private readonly byte[] _iconBytes;

        private readonly string _baseDirectory;
        private readonly string _iconPath;

        public IconHolder(string baseDirectory, string iconPath)
        {
            _baseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
            _iconPath = iconPath ?? throw new ArgumentNullException(nameof(iconPath));
        }

        public IconHolder(byte[] iconBytes)
        {
            _iconBytes = iconBytes ?? throw new ArgumentNullException(nameof(iconBytes));
        }

        public Image TryBuildImage()
        {
            Image icon;

            if (null != _iconBytes)
                icon = Deserialize(_iconBytes);
            else
            {
                if (null == _iconPath)
                    throw new InvalidOperationException("null == _iconPath");

                var iconFullPath = Path.Combine(_baseDirectory, _iconPath);

                try
                {
                    icon = Image.FromFile(iconFullPath);
                }
                catch (FileNotFoundException exception)
                {
                    Logger.Warn(exception.Message, exception);
                    icon = null;
                }
            }

            return icon;
        }

        private static Bitmap Deserialize(byte[] bitmapBytes)
        {
            Bitmap bitmap;

            using (var memoryStream = new MemoryStream(bitmapBytes))
            {
                var binaryFormatter = new BinaryFormatter();
                bitmap = (Bitmap)binaryFormatter.Deserialize(memoryStream);
            }

            return bitmap;
        }
    }
}
