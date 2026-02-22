using boletim.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace boletim.Controllers
{
    [Route("api/igreja")]
    [ApiController]
    public class IgrejaController : ControllerBase
    {
        [HttpGet("listar")]
        public IActionResult Listar()
        {
            return Ok(new List<IgrejaListagem> {
            new IgrejaListagem { Id = 1, Nome = "604 Norte" },
            new IgrejaListagem { Id = 2, Nome = "605 Norte" },
            });
        }
    }
}
