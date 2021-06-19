using System;
using System.IO;
using System.Linq;

namespace OperaFS.Utilities
{
    internal static class FileSystemManager
    {
        /// <summary>
        /// Initializes the <see cref="FileStream"/> for the file system to use.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        public static void Initialize(string filePath)
        {
            if(file != null)
            {
                throw new Exception("File already initialized!"); 
            }

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            file = new FileStream(filePath, FileMode.Open);
        }

        public static byte[] ReadBytes(long byteOffset, int byteCount)
        {
            if(file == null)
            {
                throw new Exception("File has not been initialized!");
            }

            byte[] result = new byte[byteCount];

            file.Seek(byteOffset, SeekOrigin.Begin);
            file.Read(result, 0, byteCount);

            return result;
        }

        public static bool ValidateFile()
        {
            if(file == null)
            {
                throw new Exception("File has not been initialized!");
            }

            string duck = "duck";
            byte[] actualBytes = new byte[4];

            file.Seek(132, SeekOrigin.Begin);
            file.Read(actualBytes, 0, 4);

            string actualValue = new string(actualBytes.Select(b => (char)b).ToArray());

            return actualValue == duck;
        }

        private static FileStream file;
    }
}