namespace Web.View
{
    public interface IItemProperty
    {
        string Name { get; set; }
        DateTime CreationTime { get; set; }
        string DirectoryName { get; set; }
    }
}
