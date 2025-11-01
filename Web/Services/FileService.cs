namespace Web.Services;

public class FileService
{
    public string PathMusic;
    public FileService(IWebHostEnvironment hostEnvironment)
    {

        var cachePath = Path.Combine(hostEnvironment.ContentRootPath, "FileDir");
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }
        PathMusic = cachePath;
    }
    public string GetPathInMusic(string path)
    {

        return Path.GetFullPath(Path.Combine(PathMusic , path));
    }
    
    public bool IsPathInsideBaseDirectory(string path)
    {
      
        return path.StartsWith(
            PathMusic,
            StringComparison.OrdinalIgnoreCase
        );
    }
    
    public static bool IsValidFilename(string path)
    {
        char[] chars = ['\\', '/' ];
        return chars.All((e) => !path.Contains(e)) && !string.IsNullOrWhiteSpace(path);
    }
    public (IEnumerable<string>, IEnumerable<string>)? GetItems(string path)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && Directory.Exists(realPath))
        {
            return (Directory.GetDirectories(realPath),Directory.GetFiles(realPath));
        }
        return null;
    }
    public IEnumerable<string>? GetDirectories(string path)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && Directory.Exists(realPath))
        {
            return Directory.GetDirectories(realPath);
        }
        return null;
    }
    public string? GetFile(string path)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && File.Exists(realPath))
        {
            return realPath;
        }
        return null;

    }
    public bool Delete(string path, string fileToDelete)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && IsValidFilename(fileToDelete))
        {
            var pathToDelete = Path.Combine(realPath, fileToDelete);
            if (File.Exists(pathToDelete))
            {
                File.Delete(pathToDelete);
                return true;
            }
            else if (Directory.Exists(pathToDelete))
            {
                Directory.Delete(pathToDelete, true);
                return true;
            }
                
        }
        return false;
    }

    public bool Rename(string path, string name, string newname)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && IsValidFilename(name) && IsValidFilename(newname))
        {
            var nameOrigin = Path.Combine(realPath, name);
            var nameMoved = Path.Combine(realPath, newname);

            if (File.Exists(nameOrigin) && !File.Exists(nameMoved))
            {
                File.Move(nameOrigin, nameMoved);
            } else if (Directory.Exists(nameOrigin) && !Directory.Exists(nameMoved))
            {
                Directory.Move(nameMoved, realPath);
            }

        }
        return false;
    }
    public bool CreateDirectory(string path, string name)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && IsValidFilename(name))
        {
            Directory.CreateDirectory(Path.Combine(realPath, name));

        }
        return false;
    }

    public bool Upload(string path, string name, byte[] bytes)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && IsValidFilename(name))
        {
            File.WriteAllBytes(Path.Combine(realPath, name), bytes);

        }
        return false;
    }
    public (IEnumerable<string>,IEnumerable<string>)? SearchItems(string path,string findstr)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && Directory.Exists(realPath))
        {
            var directories = Directory.GetDirectories(realPath, $"*{findstr}*", SearchOption.AllDirectories);
            var files = Directory.GetFiles(realPath, $"*{findstr}*.*", SearchOption.AllDirectories);
            return (directories, files);
        }
        return null;
    }
    public bool IsFile(string path)
    {
        var realPath = GetPathInMusic(path);
        return IsPathInsideBaseDirectory(realPath) && File.Exists(realPath);
    }
    public bool IsDirectory(string path)
    {
        var realPath = GetPathInMusic(path);
        return IsPathInsideBaseDirectory(realPath) && Directory.Exists(realPath);
    }
    public bool Exists(string path)
    {
        var realPath = GetPathInMusic(path);
        return IsPathInsideBaseDirectory(realPath) && Path.Exists(realPath);
    }
    public async Task<MemoryStream?> Download(string path,string filename)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && IsValidFilename(filename))
        {

            var pathToDownload = Path.Combine(realPath, filename);
            if (File.Exists(pathToDownload))
            {
                using FileStream fileStream = new(pathToDownload, FileMode.Open, FileAccess.Read);
                MemoryStream memoryStream = new();
                await fileStream.CopyToAsync(memoryStream);
                return memoryStream;
            }
            

        }
        return null;
    }



    public void Copy(string path, string destinationPath)
    {
        var realPath = GetPathInMusic(path);
        var realDestinationPath = GetPathInMusic(destinationPath);
        if (IsPathInsideBaseDirectory(realPath) && IsPathInsideBaseDirectory(realDestinationPath) && !Path.Exists(destinationPath))
        {
            if (File.Exists(realPath))
            {
                File.Copy(realPath, realDestinationPath);
            }
            else if (Directory.Exists(realPath))
            {
                //Directory.Copy(realPath, realDestinationPath, true);
            }
        }
    }
    public void Move(string path, string destinationPath)
    {
        var realPath = GetPathInMusic(path);
        var realDestinationPath = GetPathInMusic(destinationPath);
        if (IsPathInsideBaseDirectory(realPath) && IsPathInsideBaseDirectory(realDestinationPath) && !Path.Exists(destinationPath))
        {
            if (File.Exists(realPath))
            {
                File.Move(realPath, realDestinationPath);
            }
            else if (Directory.Exists(realPath))
            {
                Directory.Move(realPath, realDestinationPath);
            }
        }
    }
    public void GetProperty(string path, string destinationPath)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath)
            && Path.Exists(realPath))
        {
            var info = new FileInfo(realPath);
            var name = info.Name;
            var size = info.Length;
            var test = info.CreationTime;
            var d = info.DirectoryName;
            
        }
    }

}
