using Microsoft.AspNetCore.Mvc;

namespace boletim.Results
{
    public class PdfResult : ActionResult
    {
        private byte[] _bytes;
        private readonly string _nomeArquivo;

        public PdfResult(IFolhaPdf pdf)
        {
            _bytes = pdf.GerarBytes();
            _nomeArquivo = pdf.GerarNomeArquivo();
        }

        override public void ExecuteResult(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/pdf";
            response.Headers.Add("Content-Disposition", $"inline;filename={_nomeArquivo}.pdf");
            response.Body.WriteAsync(_bytes, 0, _bytes.Length);          
        }
    }

    public interface IFolhaPdf
    {
        byte[] GerarBytes();
        string GerarNomeArquivo();
    }
}
