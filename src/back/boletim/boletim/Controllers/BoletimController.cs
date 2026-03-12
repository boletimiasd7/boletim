using boletim.Models.Responses;
using boletim.Results;
using Microsoft.AspNetCore.Mvc;

namespace boletim.Controllers
{
    public class BoletimController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/boletim/gerarpdf")]
        public IActionResult GerarPDF() 
        {
            IFolhaPdf pdf = new BoletimPdf();
            return new PdfResult(pdf);
        }
    }
}
