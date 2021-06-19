using System.Collections.Generic;
using OperaFS.Utilities;

namespace OperaFS.FileSystem
{
    internal class Directory
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of this Directory.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a collection of <see cref="DirectoryHeader"/> values found in this directory.
        /// </summary>
        public List<DirectoryHeader> DirectoryHeaders { get; } = new List<DirectoryHeader>();

        /// <summary>
        /// Gets the collection of <see cref="Directory"/> values found under this directory.
        /// </summary>
        public List<Directory> Directories { get; } = new List<Directory>();

        /// <summary>
        /// Gets the collection of <see cref="DirectoryEntry"/> values found under this directory.
        /// </summary>
        public List<DirectoryEntry> Entries { get; } = new List<DirectoryEntry>();

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Directory"/> class.
        /// </summary>
        /// <param name="name">The name of this directory.</param>
        public Directory(string name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Public Static Methods

        /// <summary>
        /// Reads a <see cref="Directory"/> from the given offset.
        /// </summary>
        /// <param name="byteOffset">
        /// The offset in the file where the <see cref="DirectoryEntry"/> can be found.
        /// </param>
        /// <param name="name">
        /// The name of the directory.
        /// </param>
        /// <returns>The read <see cref="Directory"/>.</returns>
        public static Directory ReadDirectory(uint byteOffset, string name)
        {
            Directory result = new Directory(name);

            DirectoryHeader firstHeader = DirectoryHeader.ReadDirectoryHeader(byteOffset);
            result.DirectoryHeaders.Add(firstHeader);

            int nextHeaderBlock = firstHeader.NextBlock;
            while(nextHeaderBlock > 0)
            {
                uint headerLocation = byteOffset + ((uint)nextHeaderBlock * 2048);
                DirectoryHeader directoryHeader = DirectoryHeader.ReadDirectoryHeader(headerLocation);
                result.DirectoryHeaders.Add(directoryHeader);
                nextHeaderBlock = directoryHeader.NextBlock;
            }

            foreach(var header in result.DirectoryHeaders)
            {
                var entries = header.GetDirectoryEntries();

                foreach(var entry in entries)
                {
                    result.Entries.Add(entry);

                    if((entry.Flags & 7) == 7)
                    {
                        result.Directories.Add(Directory.ReadDirectory(entry.CopyOffset * 2048, entry.Name));
                    }
                }
            }

            return result;
        }

        #endregion Public Static Methods

        public override string ToString()
        {
            return $"Directory: {Name}";
        }

        
    }
}
