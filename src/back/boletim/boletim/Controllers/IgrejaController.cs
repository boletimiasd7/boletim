using boletim.Database;
using boletim.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace boletim.Controllers
{
    [Route("api/igreja")]
    [ApiController]
    public class IgrejaController : ControllerBase
    {
        private readonly BoletimDb _db;

        public IgrejaController(BoletimDb db)
        {
            _db = db;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarAsync()
        {
            List<IgrejaListagem> igrejas = await _db.Igreja.Select(x => new IgrejaListagem
            {
                Id = x.Id,
                Nome = x.Nome
            }).ToListAsync();

            return Ok(igrejas);
        }
    }
}
