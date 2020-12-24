using System.Collections.Generic;
using System.IO;
using System.Text;
using OperaFS.Utilities;

namespace OperaFS
{
    public class Root
    {
        public const int SIZE = 48;

        public Root(byte[] rootBytes)
        {
            using (MemoryStream ms = new MemoryStream(rootBytes))
            {
                using (BigEndianBinaryReader br = new BigEndianBinaryReader(ms, Encoding.ASCII))
                {
                    Id = br.ReadUInt32();
                    BlockCount = br.ReadUInt32();
                    BlockSize = br.ReadUInt32();
                    LastCopyNumber = br.ReadUInt32();

                    CopyLocations = new uint[LastCopyNumber];

                    for(int i = 0; i < CopyLocations.Length; i++)
                    {
                        CopyLocations[i] = br.ReadUInt32() * BlockSize;
                    }
                }
            }

            RootCopies = new List<Directory>();
            for (int i = 0; i < CopyLocations.Length; i++)
            {
                var firstBlock = new Directory(CopyLocations[i], "Root");
                //for(int j = 1; j < BlockCount; j++)
                //{
                //    var block = new Directory(CopyLocations[i] + (uint)(j * BlockSize), "Root");
                //    firstBlock.Directories.AddRange(block.Directories);
                //    firstBlock.Entries.AddRange(block.Entries);
                //}

                RootCopies.Add(firstBlock);
            }
        }

        public uint Id { get; }

        public uint BlockCount { get; }

        public uint BlockSize { get; }

        public uint LastCopyNumber { get; }

        public uint[] CopyLocations { get; }

        public List<Directory> RootCopies { get; }
    }
}
