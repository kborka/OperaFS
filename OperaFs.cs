using System;
using System.IO;

namespace OperaFS
{
    public static class OperaFs
    {
        private static FileStream _file;

        public static void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"Could not find file: {filePath}");
            }

            _file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public static SuperBlock Initialize()
        {
            return new SuperBlock(FetchBytes(0, SuperBlock.SIZE));
        }

        public static void WriteToDirectory(string directoryPath, SuperBlock superBlock)
        {
            if(!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }

            WriteDirectory(superBlock.Root.RootCopies[0], directoryPath);
        }

        public static byte[] FetchBytes(uint byteOffset, int size)
        {
            byte[] results = new byte[size];

            _file.Seek(byteOffset, SeekOrigin.Begin);
            _file.Read(results, 0, size);            

            return results;
        }

        private static void WriteDirectory(Directory directory, string directoryPath)
        {
            string directoryName = directory.Name.Replace("\0", string.Empty);
            string currentDirectory = $"{directoryPath}\\{directoryName}";
            if(!System.IO.Directory.Exists(currentDirectory))
            {
                System.IO.Directory.CreateDirectory(currentDirectory);
            }

            foreach(var entry in directory.Entries)
            {
                string fileName = entry.FileName.Replace("\0", string.Empty);
                string finalPath = $"{directoryPath}\\{directoryName}\\{fileName}";
                if (File.Exists(finalPath))
                {
                    continue;
                }

                using (FileStream stream = new FileStream(finalPath, FileMode.CreateNew, FileAccess.Write))
                {
                    var fileContents = FetchBytes((entry.CopyOffset * entry.BlockSize), (int)entry.ByteLength);
                    stream.Write(fileContents, 0, fileContents.Length);
                }
            }

            foreach(var subDir in directory.Directories)
            {
                WriteDirectory(subDir, currentDirectory);
            }
        }
    }
}
