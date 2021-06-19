using System;
using System.IO;
using OperaFS.FileSystem;
using OperaFS.Utilities;

namespace OperaFS
{
    public class OperaFs
    {
        public OperaFs(string filePath)
        {
            this.filePath = filePath;
        }

        public void ReadFile()
        {
            FileSystemManager.Initialize(filePath);

            if(!FileSystemManager.ValidateFile())
            {
                throw new Exception("File is not a valid OperaFS file system!");
            }

            volume = VolumeHeader.ReadVolumeHeader(0);
        }

        private VolumeHeader volume;
        private readonly string filePath;
    }
}
