namespace Web.View
{
    public class FileProperty : IItemProperty
    {
        public FileProperty(FileInfo fileInfo)
        {
            this.Name = fileInfo.Name;
            this.Length = fileInfo.Length;
            this.CreationTime = fileInfo.CreationTime;
            this.DirectoryName = fileInfo.DirectoryName ?? string.Empty;
        }
        public string Name { get; set; }
        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public string DirectoryName { get; set; }
    }
}
