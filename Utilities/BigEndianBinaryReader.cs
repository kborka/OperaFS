using System;
using System.IO;
using System.Text;

namespace OperaFS_Viewer
{
    public class BigEndianBinaryReader : BinaryReader
    {
        public BigEndianBinaryReader(Stream stream) : base(stream)
        {

        }

        public BigEndianBinaryReader(Stream stream, Encoding encoding) : base(stream, encoding)
        {

        }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public override uint ReadUInt32()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }
    }
}
