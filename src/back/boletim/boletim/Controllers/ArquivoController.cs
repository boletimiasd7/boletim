using boletim.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace boletim.Controllers
{
    [Route("api/arquivo")]
    public class ArquivoController : ControllerBase
    {
        private readonly ArquivoServico _arquivoServico;

        public ArquivoController(ArquivoServico arquivoServico)
        {
            _arquivoServico = arquivoServico;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
            stream.Seek(0, SeekOrigin.Begin);

            string nome = await _arquivoServico.Enviar(file.FileName, file.ContentType, stream);

            return Ok(nome);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string nome)
        {
            byte[] bytes = await _arquivoServico.Download(nome);

            return File(bytes, "application/octet-stream", nome);
        }
    }
}
