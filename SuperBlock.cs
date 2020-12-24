using System.IO;
using System.Text;

namespace OperaFS_Viewer.Model
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

        public Volume Volume { get; }

        public Root Root { get; }
    }
}
