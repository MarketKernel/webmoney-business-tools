using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace WMBusinessTools.Extensions.Utils
{
    internal static class FileUtility
    {
        public static string MakeRelativePath(string fileAbsolutePath, string baseDirectory)
        {
            if (null == fileAbsolutePath)
                throw new ArgumentNullException(nameof(fileAbsolutePath));

            if (null == baseDirectory)
                throw new ArgumentNullException(nameof(baseDirectory));

            fileAbsolutePath = Path.GetFullPath(fileAbsolutePath);
            baseDirectory = Path.GetFullPath(baseDirectory);

            var directorySeparator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            var altDirectorySeparator = Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

            bool isLinuxPath = fileAbsolutePath.StartsWith(directorySeparator);

            string[] separators =
            {
                directorySeparator,
                altDirectorySeparator
            };

            string[] fileAbsolutePathParts = fileAbsolutePath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string[] baseDirectoryParts = baseDirectory.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int length = Math.Min(fileAbsolutePathParts.Length, baseDirectoryParts.Length);

            int offset = 0;

            for (int i = 0; i < length; i++)
            {
                if (fileAbsolutePathParts[i]
                    .Equals(baseDirectoryParts[i],
                        isLinuxPath ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                    offset++;
                else break;
            }

            if (0 == offset)
            {
                if (!fileAbsolutePath.StartsWith(directorySeparator))
                    throw new ArgumentException("Paths do not have a common base!");
            }

            var relativePath = new StringBuilder();

            for (int i = 0; i < baseDirectoryParts.Length - offset; i++)
            {
                relativePath.Append("..");
                relativePath.Append(Path.DirectorySeparatorChar);
            }

            for (int i = offset; i < fileAbsolutePathParts.Length - 1; i++)
            {
                relativePath.Append(fileAbsolutePathParts[i]);
                relativePath.Append(Path.DirectorySeparatorChar);
            }

            relativePath.Append(fileAbsolutePathParts[fileAbsolutePathParts.Length - 1]);

            return relativePath.ToString();
        }
    }
}
