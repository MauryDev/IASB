namespace Web.View
{
    public class DirectoryProperty : IItemProperty
    {
        public DirectoryProperty(DirectoryInfo directoryInfo)
        {
            Name = directoryInfo.Name;
            CreationTime = directoryInfo.CreationTime;
            DirectoryName = directoryInfo.Parent?.FullName ?? string.Empty;
            FileCount = directoryInfo.EnumerateFiles().Count();
            SubDirectoryCount = directoryInfo.EnumerateDirectories().Count();
        }

        public string Name { get; set ; }
        public DateTime CreationTime { get; set; }
        public string DirectoryName { get; set; }
        public int FileCount { get; set; }
        public int SubDirectoryCount { get; set; }
    }
}
