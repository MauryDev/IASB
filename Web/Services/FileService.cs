using MudBlazor;

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

    public bool Upload(string path, string name, MemoryStream bytes)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath) && IsValidFilename(name))
        {
            using FileStream fileStream = new(Path.Combine(realPath, name), FileMode.Create, FileAccess.Write);
            bytes.CopyTo(fileStream);

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
    public string GetIconFile(string filename)
    {
        var pathExt = Path.GetExtension(filename).ToLower();
        if (pathExt == ".txt")
        {
            return Icons.Material.Filled.Description;
        }
        else if (pathExt == ".jpg" || pathExt == ".png" || pathExt == ".jpeg" || pathExt == ".gif" || pathExt == ".bmp")
        {
            return Icons.Material.Filled.Image;
        }
        else if (pathExt == ".mp3" || pathExt == ".wav" && pathExt == ".ogg" || pathExt == ".flac" || pathExt == ".aac")
        {
            return Icons.Material.Filled.MusicNote;
        }
        else if (pathExt == ".mp4" || pathExt == ".avi" || pathExt == ".mkv" || pathExt == ".mov")
        {
            return Icons.Material.Filled.VideoLibrary;
        }
        else if (pathExt == ".pdf")
        {
            return Icons.Material.Filled.PictureAsPdf;
        }
        else if (pathExt == ".zip" || pathExt == ".rar" || pathExt == ".7z" || pathExt == ".tar" || pathExt == ".gz")
        {
            return Icons.Material.Filled.FolderZip;
        }
        return Icons.Material.Filled.InsertDriveFile;

    }
    public View.FileProperty? GetFileProperty(string path)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath)
            && File.Exists(realPath))
        {
            var info = new FileInfo(realPath);

            View.FileProperty inforet = new(info);
            inforet.DirectoryName = Path.GetRelativePath(PathMusic, inforet.DirectoryName);
            return inforet;
        }
        return null;
    }
    public View.DirectoryProperty? GetDirectoryProperty(string path)
    {
        var realPath = GetPathInMusic(path);
        if (IsPathInsideBaseDirectory(realPath)
            && Directory.Exists(realPath))
        {
            var info = new DirectoryInfo(realPath);
            var infoRet = new View.DirectoryProperty(info);
            infoRet.DirectoryName = Path.GetRelativePath(PathMusic, infoRet.DirectoryName);

            return infoRet;

        }
        return null;
    }

}
