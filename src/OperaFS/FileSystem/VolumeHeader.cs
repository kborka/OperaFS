using System.Collections.Generic;
using System.IO;
using System.Linq;
using OperaFS.Utilities;

namespace OperaFS.FileSystem
{
    internal class VolumeHeader
    {

        #region Public Properties

        /// <summary>
        /// Gets the record type of the Volume Header.
        /// </summary>
        /// <remarks>Is always 1.</remarks>
        public byte RecordType { get; private set; }

        /// <summary>
        /// Gets the synchronization bytes of the Volume Header.
        /// </summary>
        /// <remarks>Should always be 0x5A.</remarks>
        public byte[] SyncBytes { get; private set; } 

        /// <summary>
        /// Gets the record version of the Volume Header.
        /// </summary>
        /// <remarks>Is always 1.</remarks>
        public byte RecordVersion { get; private set; }

        /// <summary>
        /// Gets the volume flags of the Volume Header.
        /// </summary>
        /// <remarks>Details about the flags are unknown.</remarks>
        public byte Flags { get; private set; }

        /// <summary>
        /// Gets the Volume's comment.
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// Gets the Volume's label.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Gets the Volume's identifier.
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Gets the Volume's block size.
        /// </summary>
        /// <remarks>Possibly always 2048.</remarks>
        public uint BlockSize { get; private set; }

        /// <summary>
        /// Gets the Volume's block count.
        /// </summary>
        /// <remarks>This is the total number of blocks in the file.</remarks>
        public uint BlockCount { get; private set; }

        /// <summary>
        /// Gets the Directory identifier for the Root directory.
        /// </summary>
        public uint RootId { get; private set; }

        /// <summary>
        /// Gets the number of blocks in the Root directory.
        /// </summary>
        public uint RootBlockCount { get; private set; }

        /// <summary>
        /// Gets the block size of the Root directory.
        /// </summary>
        /// <remarks>Possibly always the same size as the Volume block size.</remarks>
        public uint RootBlockSize { get; private set; }

        /// <summary>
        /// Gets the last copy of the root directory.
        /// </summary>
        /// <remarks>Number of copies minus 1. Possibly always 7.</remarks>
        public uint RootLastCopyNumber { get; private set; }

        /// <summary>
        /// Locations of the root directory copies.
        /// </summary>
        /// <remarks>Locations are in block offsets from the beginning of the file.</remarks>
        public uint[] RootCopyLocations { get; private set; }

        /// <summary>
        /// A collection of Root directory copies found in the file.
        /// </summary>
        public List<Directory> RootCopies { get; } = new List<Directory>();

        #endregion Public Properties

        #region Public Static Methods

        /// <summary>
        /// Reads a <see cref="VolumeHeader"/> from a given byte offset.
        /// </summary>
        /// <param name="byteOffset">
        /// The offset in the file where the <see cref="VolumeHeader"/> can be found.
        /// </param>
        /// <returns>The read <see cref="VolumeHeader"/>.</returns>
        public static VolumeHeader ReadVolumeHeader(uint byteOffset)
        {
            VolumeHeader result;
            using(MemoryStream ms = new MemoryStream(FileSystemManager.ReadBytes(byteOffset, SIZE)))
            {
                using(BinaryReader br = new BinaryReader(ms))
                {
                    // Order matters below
                    result = new VolumeHeader()
                    {
                        RecordType = br.ReadByte(),
                        SyncBytes = br.ReadBytes(SYNCBYTECOUNT),
                        RecordVersion = br.ReadByte(),
                        Flags = br.ReadByte(),
                        Comment = new string(br.ReadBytes(VOLUMEINFOCOUNT).Select(b => (char)b).ToArray()),
                        Label = new string(br.ReadBytes(VOLUMEINFOCOUNT).Select(b => (char)b).ToArray()),
                        BlockSize = br.ReadBigEndianUInt32(),
                        BlockCount = br.ReadBigEndianUInt32(),
                        RootId = br.ReadBigEndianUInt32(),
                        RootBlockCount = br.ReadBigEndianUInt32(),
                        RootBlockSize = br.ReadBigEndianUInt32(),
                        RootLastCopyNumber = br.ReadBigEndianUInt32()
                    };
                }
            }

            return result;
        }

        #endregion Public Static Methods

        #region Private Fields

        private const int SIZE = 132;
        private const int SYNCBYTECOUNT = 5;
        private const int VOLUMEINFOCOUNT = 32;

        #endregion Private Fields
    }
}
