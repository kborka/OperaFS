using System;
using System.Collections.Generic;
using System.IO;
using OperaFS.FileSystem;

namespace OperaFS.Utilities
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Gets an <see cref="Int32"/> from bytes arranged in Big Endian.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> that contains the bytes.</param>
        /// <returns>An <see cref="Int32"/> in Little Endian form.</returns>
        public static int ReadBigEndianInt32(this BinaryReader br)
        {
            var data = br.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// Gets a <see cref="UInt32"/> from bytes arranged in Big Endian.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> that contains the bytes.</param>
        /// <returns>A <see cref="UInt32"/> in Little Endian form.</returns>
        public static uint ReadBigEndianUInt32(this BinaryReader br)
        {
            var data = br.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        /// <summary>
        /// Gets all <see cref="DirectoryEntry"/> values from the <see cref="DirectoryHeader"/>.
        /// </summary>
        /// <param name="header">The <see cref="DirectoryHeader"/> to fetch the <see cref="DirectoryEntry"/> values from.</param>
        /// <returns>A collection of <see cref="DirectoryEntry"/> values.</returns>
        public static IList<DirectoryEntry> GetDirectoryEntries(this DirectoryHeader header)
        {
            List<DirectoryEntry> entries = new List<DirectoryEntry>();

            uint entryOffset = 0;
            while(entryOffset < header.FirstFree)
            {

                DirectoryEntry entry = DirectoryEntry.GetDirectoryEntry(header.ByteOffset + header.FirstEntry + entryOffset);
                entries.Add(entry);
                entryOffset += entry.Size;
            }

            return entries;
        }
    }
}
