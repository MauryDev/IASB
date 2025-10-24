

namespace Web.Services;

public class FileController
{
    public string PathMusic;
    public FileController(IWebHostEnvironment hostEnvironment)
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
    public (IEnumerable<string>, IEnumerable<string>)? GetMusics(string path)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && Directory.Exists(realPath))
        {
            return (Directory.GetDirectories(realPath),Directory.GetFiles(realPath));
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
}
