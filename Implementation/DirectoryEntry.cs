using System.IO;
using System.Linq;
using System.Text;
using OperaFS.Utilities;

namespace OperaFS.Implementation
{
    internal class DirectoryEntry
    {

        public const int BASIC_SIZE = 72;

        private const int FILENAMELENGTH = 32;
        private const int ENTRYTYPESIZE = 4;

        public DirectoryEntry(byte[] entryBytes)
        {
            using (MemoryStream ms = new MemoryStream(entryBytes))
            {
                using (BigEndianBinaryReader br = new BigEndianBinaryReader(ms, Encoding.BigEndianUnicode))
                {
                    Flags = br.ReadUInt32();
                    Id = br.ReadUInt32();
                    EntryType = new string(br.ReadBytes(ENTRYTYPESIZE).Select(x => (char)x).ToArray());
                    BlockSize = br.ReadUInt32();
                    ByteLength = br.ReadUInt32();
                    BlockLength = br.ReadUInt32();
                    Burst = br.ReadUInt32();
                    Gap = br.ReadUInt32();
                    FileName = new string(br.ReadBytes(FILENAMELENGTH).Select(x => (char)x).ToArray());
                    LastCopyNumber = br.ReadUInt32();

                    CopyOffset = br.ReadUInt32();//new uint[LastCopyNumber];
                    //for(int i = 0; i < CopyOffset.Length; i++)
                    //{
                    //    CopyOffset[i] = br.ReadUInt32();
                    //}
                }
            }
        }

        //flags
        //The least significant byte seems to be
        //0x02 - File
        //0x06 - Special file
        //0x07 - Directory
        //This is OR'ed with (one or more of):       
        //0x40000000 - this is the last dir entry in the block
        //0x80000000 - this is the last dir entry of the dir
        public uint Flags { get; }

        public uint Id { get; }

        //entry type
        //"*dir" - directory
        //"*lbl" - label(points to volume header)
        //"*zap" - catapult(fast startup information)
        //something else - file.Seems to be the last 4 letters
        //of the extension, right padded with spaces if less.
        //Sometimes the case is retained, sometimes it is
        //converted to lowercase.
        public string EntryType { get; }

        public uint BlockSize { get; }

        public uint ByteLength { get; }

        public uint BlockLength { get; }

        public uint Burst { get; }

        public uint Gap { get; }

        public string FileName { get; }

        public uint LastCopyNumber { get; }

        public uint CopyOffset { get; }

        public uint Size => 68 + 4*(LastCopyNumber + 1);

        public override string ToString()
        {
            return $"Entry: {FileName}";
        }
    }
}
