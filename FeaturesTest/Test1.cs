using System.IO.Compression;
using System.Text.RegularExpressions;

namespace FeaturesTest
{
    [TestClass]
    public sealed class Test1
    {
        public static string ExtractYouTubeVideoId(string url)
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
        public static bool IsPathInsideBaseDirectory(string basePath,string inputPath)
        {
            string canonicalInput = Path.GetFullPath(basePath + inputPath);

            return canonicalInput.StartsWith(
                basePath,
                StringComparison.OrdinalIgnoreCase
            ) && canonicalInput.Length != basePath.Length;
        }
        [TestMethod]
        public void GetParent()
        {
            var t = Path.GetDirectoryName("ad");
            Assert.AreEqual("ad",t);
        }
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(IsPathInsideBaseDirectory(Path.GetFullPath("Music") + "\\", "a"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var url = "https://harmonious-piroshki-28fc2f.netlify.app/Informativo/informativo_251025_alta.mp4";

            var uri = new Uri(url);
            var path = uri.LocalPath;
            var filename = Path.GetFileName(path);
            Assert.AreEqual("informativo_251025_alta.mp4", filename);
        }

        [TestMethod]
        public void TestZip()
        {
            var path = "C:\\Users\\Maury\\Downloads\\informativo_251025_alta.zip";
            using FileStream fstream = new (path, FileMode.Open);
            using ZipArchive zipArchive = new(fstream);

            zipArchive.Entries.ToList().ForEach(entry =>
            {
                Console.WriteLine(entry.FullName);
            });
        }
        [TestMethod]
        public void Youtube()
        {
            string[] urls = new string[]
            {
                "https://www.youtube.com/watch?v=FPoN-5VMo2g",
                "https://youtu.be/FPoN-5VMo2g",
                "https://www.youtube.com/embed/FPoN-5VMo2g?si=n9NGA0gaoGw2V7Ux"
            };
            foreach (var url in urls)
            {
                var videoId = ExtractYouTubeVideoId(url);
                Assert.AreEqual("FPoN-5VMo2g", videoId);
            }
        }
    }
}
