using System.IO;
using System.Linq;

namespace OperaFS_Viewer.Model
{
    public class Volume
    {
        public const int SIZE = 83;

        private const int SYNCBYTECOUNT = 5;
        private const int VOLUMEINFOCOUNT = 32;

        public Volume(byte[] volumeBytes)
        {
            using (MemoryStream ms = new MemoryStream(volumeBytes))
            {
                using (BigEndianBinaryReader br = new BigEndianBinaryReader(ms))
                {
                    SyncBytes = br.ReadBytes(SYNCBYTECOUNT);
                    RecordVersion = br.ReadByte();
                    Flags = br.ReadByte();
                    Comment = new string(br.ReadBytes(VOLUMEINFOCOUNT).Select(x => (char)x).ToArray());
                    Label = new string(br.ReadBytes(VOLUMEINFOCOUNT).Select(x => (char)x).ToArray());
                    Id = br.ReadUInt32();
                    BlockSize = br.ReadUInt32();
                    BlockCount = br.ReadUInt32();
                }
            }
        }

        public byte[] SyncBytes { get; }

        public byte RecordVersion { get; }

        public byte Flags { get; }

        public string Comment { get; }

        public string Label { get; }

        public uint Id { get; }

        public uint BlockSize { get; }

        public uint BlockCount { get; }
    }
}
