using System.IO;
using System.Text;
using OperaFS.Utilities;

namespace OperaFS.FileSystem
{
    internal class DirectoryHeader
    {
        #region Public Properties

        /// <summary>
        /// Gets the byte offset in the file that this <see cref="DirectoryHeader"/> is found.
        /// </summary>
        public uint ByteOffset { get; private set; }

        /// <summary>
        /// Gets the location of the next block in this directory.
        /// </summary>
        /// <remarks>This value is <c>0xffffffff</c> if this is the last block.</remarks>
        public int NextBlock { get; private set; }

        /// <summary>
        /// Gets the location of the previous block in this directory.
        /// </summary>
        /// <remarks>This value is <c>0xffffffff</c> if this is the first block.</remarks>
        public int PrevBlock { get; private set; }

        /// <summary>
        /// Gets the flags associated to this <see cref="DirectoryHeader"/>
        /// </summary>
        /// <remarks>Details unknown.</remarks>
        public uint Flags { get; private set; }

        /// <summary>
        /// Gets the offset from the beginning of the block to the first unused byte in the block.
        /// </summary>
        public uint FirstFree { get; private set; }

        /// <summary>
        /// Gets the offset from the beginning of the block to the first directory entry in this block.
        /// </summary>
        /// <remarks>Is possibly always <c>0x14</c>.</remarks>
        public uint FirstEntry { get; private set; }

        #endregion Public Properties

        #region Public Static Methods

        /// <summary>
        /// Reads a <see cref="DirectoryHeader"/> from the given offset.
        /// </summary>
        /// <param name="byteOffset">
        /// The offset in the file where the <see cref="DirectoryHeader"/> can be found.
        /// </param>
        /// <returns>The read <see cref="DirectoryHeader"/>.</returns>
        public static DirectoryHeader ReadDirectoryHeader(uint byteOffset)
        {
            DirectoryHeader result;

            using(MemoryStream ms = new MemoryStream(FileSystemManager.ReadBytes(byteOffset, SIZE)))
            {
                using(BinaryReader br = new BinaryReader(ms, Encoding.ASCII))
                {
                    // Order matters below.
                    result = new DirectoryHeader()
                    {
                        ByteOffset = byteOffset,
                        NextBlock = br.ReadBigEndianInt32(),
                        PrevBlock = br.ReadBigEndianInt32(),
                        Flags = br.ReadBigEndianUInt32(),
                        FirstFree = br.ReadBigEndianUInt32(),
                        FirstEntry = br.ReadBigEndianUInt32()
                    };
                }
            }

            return result;
        }

        #endregion Public Static Methods

        #region Private Fields

        private const int SIZE = 20;

        #endregion Private Fields
    }
}