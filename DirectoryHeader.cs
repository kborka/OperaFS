using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperaFS.Utilities;

namespace OperaFS
{
    public class DirectoryHeader
    {
        public const int SIZE = 20;

        private uint _fileOffsetLocation;

        public DirectoryHeader(byte[] headerBytes, uint fileOffsetLocation)
        {
            _fileOffsetLocation = fileOffsetLocation;
            Entries = new List<DirectoryEntry>();
            using (MemoryStream ms = new MemoryStream(headerBytes))
            {
                using (BigEndianBinaryReader br = new BigEndianBinaryReader(ms, Encoding.ASCII))
                {
                    NextBlock = br.ReadInt32();
                    PrevBlock = br.ReadInt32();
                    Flags = br.ReadUInt32();
                    FirstFree = br.ReadUInt32();
                    FirstEntry = br.ReadUInt32();
                }
            }

            uint entryOffset = 0;
            while (entryOffset < FirstFree)
            {
                var entry = new DirectoryEntry(OperaFs.FetchBytes(_fileOffsetLocation + FirstEntry + entryOffset, DirectoryEntry.BASIC_SIZE));
                Entries.Add(entry);
                entryOffset += entry.Size;
            }
        }

        public int NextBlock { get; }

        public int PrevBlock { get; }

        public uint Flags { get; }

        public uint FirstFree { get; }

        public uint FirstEntry { get; }

        public List<DirectoryEntry> Entries { get; }
    }
}
