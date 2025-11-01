using SharpYaml.Serialization;
using System;
using Web.View;

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
    private static Uri BASEURLAPI = new("https://iasb.api.iasdbenedito.dedyn.io/provai e vede/");

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
    public async Task<View.ProvaiVede?> GetInfoLocal()
    {
        var localPath = Path.Combine(_cachedVideosPath, "provai-vede.yml");
        if (!File.Exists(localPath))
        {
            return null;
        }

        var localContent = await File.ReadAllTextAsync(localPath);
        var serializer = new Serializer();
        var provaiVede = serializer.Deserialize<View.ProvaiVede>(localContent);

        return provaiVede;
    }
    public async Task SetInfoLocal(View.ProvaiVede provaiVede)
    {
        var localPath = Path.Combine(_cachedVideosPath, "provai-vede.yml");

        var serializer = new Serializer();

        if (File.Exists(localPath))
        {
            await File.WriteAllTextAsync(localPath, serializer.Serialize(provaiVede));

        }
    }
    public async Task<View.ProvaiVede?> GetInfoServer()
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
        var provaiVede = serializer.Deserialize<View.ProvaiVede>(content);
        return provaiVede;
    }
    public async Task<View.ProvaiVede?> GetInfo()
    {
        var cloud = await GetInfoServer();
        if (cloud == null)
        {


           
            return await GetInfoLocal();
        } else
        {
            var localProvaiVede = await GetInfoLocal();

            if (localProvaiVede != null && cloud.Date != localProvaiVede.Date)
            {
                await SetInfoLocal(cloud);
                DeleteCache(localProvaiVede);
            }
            else
            {
                await SetInfoLocal(cloud);

            }
            return cloud;
        }
    }
    public string? GetFileNameVideo(View.ProvaiVede provaiVede)
    {
        var uri = new Uri(provaiVede.Url);
        var path = uri.LocalPath;
        var filename = Path.GetFileName(path);
        return filename;
    }
    public void DeleteCache(View.ProvaiVede provaiVede)
    {
        var localPath = Path.Combine(_cachedVideosPath, GetFileNameVideo(provaiVede));
        if (File.Exists(localPath))
        {
            File.Delete(localPath);
        }
    }
    public bool PlayLocal(View.ProvaiVede provaiVede)
    {

        var localPath = Path.Combine(_cachedVideosPath, GetFileNameVideo(provaiVede));
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
    public async Task DownloadVideo(View.ProvaiVede provaiVede)
    {
        var url = provaiVede.Url;
        var filename = GetFileNameVideo(provaiVede);
        var stream = await _httpClient.GetStreamAsync(url);
        var localPath = Path.Combine(_cachedVideosPath, filename);
        using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream);
    }
    public async Task Download(View.ProvaiVede provaiVede, Func<string, Task>? OnMessage)
    {
        var localPath = Path.Combine(_cachedVideosPath, GetFileNameVideo(provaiVede));
        if (File.Exists(localPath))
            return;
        await OnMessage("Baixando o vídeo no servidor...");

        try
        {
            await DownloadVideo(provaiVede);
        } catch { }
    }
    public async Task Play(View.ProvaiVede provaiVede, Func<string,Task> OnMessage)
    {
        await OnMessage("Tentando abrir...");
        if (PlayLocal(provaiVede))
            return;
        await OnMessage("Baixando o vídeo no servidor...");

        try
        {
            await DownloadVideo(provaiVede);
            await OnMessage("Tentando abrir...");

            PlayLocal(provaiVede);
        }
        catch { }
    }
}