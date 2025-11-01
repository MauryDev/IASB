using SharpYaml.Serialization;
using System;

namespace Web.Services;

/// <summary>
/// Representa as informações detalhadas de um informativo, incluindo metadados e URL do vídeo.
/// </summary>


public class ProvaiVedeService
{
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<InformativoService> _logger; // Para logs
    private const string CACHE_FOLDER_NAME = "ProvaiVede-Cache";
    private readonly string _cachedVideosPath; // Caminho absoluto para a pasta de cache
    private static Uri BASEURLAPI = new("https://harmonious-piroshki-28fc2f.netlify.app/provai e vede/");

    public ProvaiVedeService(
        HttpClient httpClient,
        IWebHostEnvironment webHostEnvironment,
        ILogger<InformativoService> logger)
    {
        _httpClient = httpClient;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;

        // Inicializa o caminho do cache no construtor
        _cachedVideosPath = Path.Combine(webHostEnvironment.ContentRootPath, CACHE_FOLDER_NAME);

        // Garante que o diretório de cache existe
        if (!Directory.Exists(_cachedVideosPath))
        {
            Directory.CreateDirectory(_cachedVideosPath);
            _logger.LogInformation("Pasta de cache de vídeos criada em: {Path}", _cachedVideosPath);
        }
    }
    public async Task<View.Informativo?> GetInfoLocal()
    {
        var localPath = Path.Combine(_cachedVideosPath, "provai-vede.yml");
        if (!File.Exists(localPath))
        {
            return null;
        }

        var localContent = await File.ReadAllTextAsync(localPath);
        var serializer = new Serializer();
        var informativo = serializer.Deserialize<View.Informativo>(localContent);

        return informativo;
    }
    public async Task SetInfoLocal(View.Informativo informativo)
    {
        var localPath = Path.Combine(_cachedVideosPath, "provai-vede.yml");

        var serializer = new Serializer();

        await File.WriteAllTextAsync(localPath, serializer.Serialize(informativo));
        if (File.Exists(localPath))
        {
        }
    }
    public async Task<View.Informativo?> GetInfoServer()
    {
        var url = new Uri(BASEURLAPI, "index.yml");
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Falha ao obter o provai e vede. Status Code: {StatusCode}", response.StatusCode);
            return null;
        }
        var content = await response.Content.ReadAsStringAsync();
        var serializer = new Serializer();
        var informativo = serializer.Deserialize<View.Informativo>(content);
        return informativo;
    }
    public async Task<View.Informativo?> GetInfo()
    {
        var cloud = await GetInfoServer();
        if (cloud == null)
        {


            var localInformativo = await GetInfoLocal();
           
            return localInformativo;
        } else
        {
            var localInformativo = await GetInfoLocal();

            if (localInformativo != null && cloud.Date != localInformativo.Date)
            {
                await SetInfoLocal(cloud);
                DeleteCache(localInformativo);
            }
            else
            {
                await SetInfoLocal(cloud);

            }
            return cloud;
        }
    }
    public string? GetFileNameVideo(View.Informativo informativo)
    {
        var uri = new Uri(informativo.Url);
        var path = uri.LocalPath;
        var filename = Path.GetFileName(path);
        return filename;
    }
    public void DeleteCache(View.Informativo informativo)
    {
        var localPath = Path.Combine(_cachedVideosPath, GetFileNameVideo(informativo));
        if (File.Exists(localPath))
        {
            File.Delete(localPath);
        }
    }
    public bool PlayLocal(View.Informativo informativo)
    {

        var localPath = Path.Combine(_cachedVideosPath, GetFileNameVideo(informativo));
        if (File.Exists(localPath))
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = localPath,
                UseShellExecute = true
            });
            return true;
        }
        return false;

    }
    public async Task DownloadVideo(View.Informativo informativo)
    {
        var url = informativo.Url;
        var filename = GetFileNameVideo(informativo);
        var stream = await _httpClient.GetStreamAsync(url);
        var localPath = Path.Combine(_cachedVideosPath, filename);
        using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream);
    }
    public async Task Download(View.Informativo informativo, Func<string, Task>? OnMessage)
    {
        var localPath = Path.Combine(_cachedVideosPath, GetFileNameVideo(informativo));
        if (File.Exists(localPath))
            return;
        await OnMessage("Baixando o vídeo no servidor...");

        try
        {
            await DownloadVideo(informativo);
        } catch { }
    }
    public async Task Play(View.Informativo informativo,Func<string,Task> OnMessage)
    {
        await OnMessage("Tentando abrir...");
        if (PlayLocal(informativo))
            return;
        await OnMessage("Baixando o vídeo no servidor...");

        try
        {
            await DownloadVideo(informativo);
            await OnMessage("Tentando abrir...");

            PlayLocal(informativo);
        }
        catch { }
    }
}