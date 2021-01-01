using System.Collections.Generic;

namespace OperaFS.Implementation
{
    internal class Directory
    {
        private List<DirectoryHeader> _headers;
        
        public Directory(uint directoryByteLocation, string name)
        {
            Name = name;
            Directories = new List<Directory>();
            Entries = new List<DirectoryEntry>();
            _headers = new List<DirectoryHeader>();
            DirectoryHeader firstHeader = new DirectoryHeader(OperaFs.FetchBytes(directoryByteLocation, DirectoryHeader.SIZE), directoryByteLocation);
            _headers.Add(firstHeader);

            int nextHeaderBlock = firstHeader.NextBlock;
            while(nextHeaderBlock > 0)
            {
                uint headerLocation = directoryByteLocation + ((uint)nextHeaderBlock * 2048);
                DirectoryHeader header = new DirectoryHeader(OperaFs.FetchBytes(headerLocation, DirectoryHeader.SIZE), headerLocation);
                nextHeaderBlock = header.NextBlock;
                _headers.Add(header);
            }

            foreach(var header in _headers)
            {
                foreach(var entry in header.Entries)
                {
                    if ((entry.Flags & 7) == 7)
                    {
                        Directories.Add(new Directory(entry.CopyOffset * 2048, entry.FileName));
                    }
                    else if((entry.Flags & 2) == 2)
                    {
                        Entries.Add(entry);
                    }
                }
            }
        }

        public string Name { get; }

        public List<Directory> Directories { get; }

        public List<DirectoryEntry> Entries { get; }

        public override string ToString()
        {
            return $"Directory: {Name}";
        }
    }
}
