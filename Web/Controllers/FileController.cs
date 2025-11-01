using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Web.Services;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        FileService fileService;
        public FileController(FileService fileService)
        {
            //api/file/download?path=&filename=
            this.fileService = fileService;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Download([FromQuery] string? path, [FromQuery] string filename)
        {
            var memoryStream = await fileService.Download(path ?? string.Empty, filename);
            if (memoryStream == null)
            {
                return NotFound();
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/x-binary")
            {
                FileDownloadName = filename
            }; ;
        }
    }
}
