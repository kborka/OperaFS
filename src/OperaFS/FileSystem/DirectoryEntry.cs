using System.IO;
using System.Linq;
using System.Text;
using OperaFS.Utilities;

namespace OperaFS.FileSystem
{
    internal class DirectoryEntry
    {
        #region Public Properties

        /// <summary>
        /// Gets the flags associated with this directory entry.
        /// </summary>
        /// <remarks>
        /// <para>Possible Flags:</para>
        /// <para>The least significant byte seems to be:</para>
        /// <para>0x02 - File</para>
        /// <para>0x06 - Special file</para>
        /// <para>0x07 - Directory</para>
        /// <para>This is OR'ed with (one or more of):</para>
        /// <para>0x40000000 - this is the last dir entry in the block</para>
        /// <para>0x80000000 - this is the last dir entry of the dir</para>
        /// </remarks>
        public uint Flags { get; private set; }

        /// <summary>
        /// Gets the identifier for this directory entry.
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Gets the type of this directory entry.
        /// </summary>
        /// <remarks>
        /// <para>Possible entry types:</para>
        /// <para>"*dir" - directory</para>
        /// <para>"*lbl" - label(points to volume header)</para>
        /// <para>"*zap" - catapult(fast startup information)</para>
        /// <para>something else - file. Seems to be the last 4 letters
        /// of the extension, right padded with spaces if less.
        /// Sometimes the case is retained, sometimes it is
        /// converted to lowercase.</para>
        /// </remarks>
        public string EntryType { get; private set; }

        /// <summary>
        /// Gets the block size of this directory entry.
        /// </summary>
        /// <remarks>Possibly always the same as the <see cref="Volume.BlockSize"/>.</remarks>
        public uint BlockSize { get; private set; }

        /// <summary>
        /// Gets the length of the entry in bytes.
        /// </summary>
        public uint ByteLength { get; private set; }

        /// <summary>
        /// Gets the length of the entry in blocks.
        /// </summary>
        public uint BlockLength { get; private set; }

        /// <summary>
        /// Gets the burst value.
        /// </summary>
        /// <remarks>Purpose unknown.</remarks>
        public uint Burst { get; private set; }

        /// <summary>
        /// Gets the gap value.
        /// </summary>
        /// <remarks>Purpose unknown.</remarks>
        public uint Gap { get; private set; }

        /// <summary>
        /// Gets the name of this directory entry.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the number of the last copy of the directory entry.
        /// </summary>
        /// <remarks>Value will be number of copies minus 1.</remarks>
        public uint LastCopyNumber { get; private set; }

        /// <summary>
        /// Gets the offset of the directory entry copies.
        /// </summary>
        /// <remarks>Value is in blocks from the beginning of the file.</remarks>
        public uint CopyOffset { get; private set; }

        /// <summary>
        /// Gets the actual size of this directory entry.
        /// </summary>
        public uint Size => 68 + 4*(LastCopyNumber + 1);

        #endregion Public Properties

        #region Public Static Methods

        /// <summary>
        /// Reads a <see cref="DirectoryEntry"/> from the given offset.
        /// </summary>
        /// <param name="byteOffset">
        /// The offset in the file where the <see cref="DirectoryEntry"/> can be found.
        /// </param>
        /// <returns>The read <see cref="DirectoryEntry"/>.</returns>
        public static DirectoryEntry GetDirectoryEntry(uint byteOffset)
        {
            DirectoryEntry result;

            using(MemoryStream ms = new MemoryStream(FileSystemManager.ReadBytes(byteOffset, BASIC_SIZE)))
            {
                using(BinaryReader br = new BinaryReader(ms))
                {
                    result = new DirectoryEntry()
                    {
                        Flags = br.ReadUInt32(),
                        Id = br.ReadUInt32(),
                        EntryType = new string(br.ReadBytes(ENTRYTYPESIZE).Select(x => (char)x).ToArray()),
                        BlockSize = br.ReadUInt32(),
                        ByteLength = br.ReadUInt32(),
                        BlockLength = br.ReadUInt32(),
                        Burst = br.ReadUInt32(),
                        Gap = br.ReadUInt32(),
                        Name = new string(br.ReadBytes(NAMELENGTH).Select(x => (char)x).ToArray()),
                        LastCopyNumber = br.ReadUInt32(),
                        CopyOffset = br.ReadUInt32()
                    };
                }
            }

            return result;
        }

        #endregion Public Static Methods

        public override string ToString()
        {
            return $"Entry: {Name}";
        }

        #region Private Fields

        private const int BASIC_SIZE = 72;
        private const int NAMELENGTH = 32;
        private const int ENTRYTYPESIZE = 4;

        #endregion Private Fields
    }
}
