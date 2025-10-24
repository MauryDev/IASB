using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting; // Necessário para IWebHostEnvironment
using Microsoft.Extensions.Logging; // Opcional, mas útil para logging

namespace Web.Services;

/// <summary>
/// Representa as informações detalhadas de um informativo, incluindo metadados e URL do vídeo.
/// </summary>


public class InformativoController
{
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<InformativoController> _logger; // Para logs
    private const string CACHE_FOLDER_NAME = "Informativo-Cache";
    private readonly string _cachedVideosPath; // Caminho absoluto para a pasta de cache

    public InformativoController(
        HttpClient httpClient,
        IWebHostEnvironment webHostEnvironment,
        ILogger<InformativoController> logger)
    {
        _httpClient = httpClient;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;

        // Inicializa o caminho do cache no construtor
        _cachedVideosPath = Path.Combine(_webHostEnvironment.ContentRootPath, CACHE_FOLDER_NAME);

        // Garante que o diretório de cache existe
        if (!Directory.Exists(_cachedVideosPath))
        {
            Directory.CreateDirectory(_cachedVideosPath);
            _logger.LogInformation("Pasta de cache de vídeos criada em: {Path}", _cachedVideosPath);
        }
    }

    /// <summary>
    /// Simula a obtenção de dados de um informativo.
    /// Em uma aplicação real, isto buscaria de um banco de dados, arquivo YAML ou API.
    /// </summary>
    /// <returns>Um objeto Informativo com os dados, ou null se não for encontrado.</returns>
    public Task<View.Informativo> GetInformativoInfoAsync()
    {
        // Exemplo de um objeto Informativo mockado
        // Correspondente ao formato "yml like" do usuário
        
        var mockInformativo = new View.Informativo(
            Date: new DateTime(2025, 10, 20),
            Name: "Visão da Semana - Edição Especial de Missões",
            Description: "Este informativo traz as últimas notícias e um estudo aprofundado sobre nossas missões globais, destacando os desafios e as vitórias alcançadas em diferentes partes do mundo.",
            Size: "2.5 MB",
            Duration: "5 minutos e 12 segundos",
            Url: "https://freetestdata.com/wp-content/uploads/2021/09/MP4-Video-Test-File-Download.mp4" // URL de exemplo de um MP4
        );
        _logger.LogDebug("Informações do informativo mockadas retornadas: {Name}", mockInformativo.Name);
        return Task.FromResult(mockInformativo);
    }

    /// <summary>
    /// Verifica se o vídeo correspondente a uma URL remota já está cacheado localmente.
    /// Esta é a funcionalidade 'CanUpdateCache' focada na verificação de existência local.
    /// </summary>
    /// <param name="remoteVideoUrl">A URL do vídeo remoto.</param>
    /// <returns>True se o vídeo estiver cacheado, false caso contrário.</returns>
    public bool IsVideoCached(string remoteVideoUrl)
    {
        if (string.IsNullOrEmpty(remoteVideoUrl)) return false;

        var localFilePath = GetLocalVideoFilePath(remoteVideoUrl);
        var isCached = System.IO.File.Exists(localFilePath);
        _logger.LogDebug("Verificando cache para {Url}. Cached: {IsCached}", remoteVideoUrl, isCached);
        return isCached;
    }

    /// <summary>
    /// Baixa um vídeo MP4 de uma URL remota e o salva na pasta de cache local.
    /// Esta é a implementação da funcionalidade 'DownloadInformativoI'.
    /// </summary>
    /// <param name="remoteVideoUrl">A URL do vídeo a ser baixado.</param>
    /// <returns>True se o download e o cache foram bem-sucedidos, false caso contrário.</returns>
    public async Task<bool> DownloadInformativoVideoAsync(string remoteVideoUrl)
    {
        if (string.IsNullOrEmpty(remoteVideoUrl))
        {
            _logger.LogWarning("Tentativa de baixar vídeo com URL vazia.");
            return false;
        }

        var localFilePath = GetLocalVideoFilePath(remoteVideoUrl);

        if (IsVideoCached(remoteVideoUrl))
        {
            _logger.LogInformation("Vídeo de {Url} já está cacheado em {Path}.", remoteVideoUrl, localFilePath);
            return true;
        }

        _logger.LogInformation("Iniciando download de {Url} para {Path}...", remoteVideoUrl, localFilePath);
        try
        {
            var response = await _httpClient.GetAsync(remoteVideoUrl);
            response.EnsureSuccessStatusCode(); // Lança exceção se o código de status HTTP for um erro

            await using (var contentStream = await response.Content.ReadAsStreamAsync())
            await using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await contentStream.CopyToAsync(fileStream);
            }
            _logger.LogInformation("Download e cache concluídos para {Url}. Salvo em {Path}.", remoteVideoUrl, localFilePath);
            return true;
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "Erro HTTP ao baixar vídeo de {Url}: {Message}", remoteVideoUrl, httpEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao baixar ou salvar vídeo de {Url}: {Message}", remoteVideoUrl, ex.Message);
        }
        return false;
    }

    /// <summary>
    /// Retorna a URL relativa que pode ser usada para acessar o vídeo cacheado pelo navegador.
    /// Esta é a funcionalidade 'OpenLocalInformativo', retornando o caminho para abri-lo via HTTP.
    /// </summary>
    /// <param name="remoteVideoUrl">A URL original do vídeo remoto.</param>
    /// <returns>A URL relativa do vídeo cacheado (ex: /CachedVideos/hashedfilename.mp4)
    /// ou null se o vídeo não estiver cacheado ou a URL for inválida.</returns>
    public string? GetCachedVideoRelativeUrl(string remoteVideoUrl)
    {
        if (string.IsNullOrEmpty(remoteVideoUrl) || !IsVideoCached(remoteVideoUrl))
        {
            _logger.LogWarning("Não é possível obter a URL relativa do cache para {Url}. Vídeo não cacheado ou URL inválida.", remoteVideoUrl);
            return null;
        }

        var fileName = GetLocalVideoFileName(remoteVideoUrl);
        var relativeUrl = $"/{CACHE_FOLDER_NAME}/{fileName}";
        _logger.LogDebug("URL relativa do cache para {Url}: {RelativeUrl}", remoteVideoUrl, relativeUrl);
        return relativeUrl;
    }

    /// <summary>
    /// Gera um nome de arquivo local seguro para o vídeo com base na sua URL remota.
    /// Usa um hash SHA256 da URL para garantir unicidade e evitar caracteres inválidos.
    /// </summary>
    /// <param name="remoteVideoUrl">A URL do vídeo remoto.</param>
    /// <returns>Um nome de arquivo único para o vídeo cacheado.</returns>
    private string GetLocalVideoFileName(string remoteVideoUrl)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(remoteVideoUrl));
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            // Tenta preservar a extensão de arquivo original se disponível.
            // Se não houver, assume .mp4.
            string extension = Path.GetExtension(new Uri(remoteVideoUrl).LocalPath);
            if (string.IsNullOrEmpty(extension) || extension.Length > 5) // Evita extensões muito longas ou faltantes
            {
                extension = ".mp4";
            }
            string fileName = $"{hashString}{extension}";
            _logger.LogTrace("Nome de arquivo local gerado para {Url}: {FileName}", remoteVideoUrl, fileName);
            return fileName;
        }
    }

    /// <summary>
    /// Constrói o caminho completo do arquivo local para uma dada URL de vídeo remoto.
    /// </summary>
    /// <param name="remoteVideoUrl">A URL do vídeo remoto.</param>
    /// <returns>O caminho absoluto onde o vídeo deve ser cacheado.</returns>
    private string GetLocalVideoFilePath(string remoteVideoUrl)
    {
        var fileName = GetLocalVideoFileName(remoteVideoUrl);
        var filePath = Path.Combine(_cachedVideosPath, fileName);
        _logger.LogTrace("Caminho completo do arquivo local para {Url}: {FilePath}", remoteVideoUrl, filePath);
        return filePath;
    }
}