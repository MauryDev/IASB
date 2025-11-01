using System.Text.RegularExpressions;

namespace Web.Services
{
    public class YoutubeService
    {
        public string? ExtractId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            // 1. Tenta extrair o ID de URLs curtas, de incorporação (embed) ou de watch (v=ID)
            var reg = new Regex(@"(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))([^&?]+)");
            var match = reg.Match(url);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // 2. Se a primeira regex falhar (mais comum para vídeos curtos e watch), tenta extrair de outras variações
            try
            {
                var uri = new Uri(url);

                // Para formato 'watch?v=ID'
                if (uri.Host.Contains("youtube.com") && !string.IsNullOrEmpty(uri.Query))
                {
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    var v = query["v"];
                    if (!string.IsNullOrEmpty(v))
                    {
                        return v;
                    }
                }

                // Para formato 'youtu.be/ID'
                if (uri.Host.Contains("youtu.be"))
                {
                    return uri.AbsolutePath.Trim('/');
                }

                // Para formato 'youtube.com/embed/ID'
                if (uri.AbsolutePath.StartsWith("/embed/"))
                {
                    return uri.AbsolutePath.TrimStart("/embed/".ToCharArray()).Split('/')[0];
                }
            }
            catch (UriFormatException)
            {
                // URL inválida
                return null;
            }

            return null;
        }

    }
}
