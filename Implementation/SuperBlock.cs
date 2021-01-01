using System.IO;
using System.Text;
using OperaFS.Utilities;

namespace OperaFS.Implementation
{
    public class SuperBlock
    {
        public const int SIZE = 132;

        public SuperBlock(byte[] superBlockBytes)
        {            
            using (MemoryStream ms = new MemoryStream(superBlockBytes))
            {
                using (BigEndianBinaryReader br = new BigEndianBinaryReader(ms, Encoding.ASCII))
                {
                    VersionNumber = br.ReadByte();
                }
            }

            Volume = new Volume(OperaFs.FetchBytes(1, Volume.SIZE));
            Root = new Root(OperaFs.FetchBytes(Volume.SIZE + 1, Root.SIZE));
        }

        public byte VersionNumber { get; }

        internal Volume Volume { get; }

        internal Root Root { get; }
    }
}
