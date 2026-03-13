using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace boletim.Servicos
{
    public class ArquivoServico
    {
        private readonly StorageClient _storageClient;
        private const string BUCKET_NAME = "boletim_iasd";

        public ArquivoServico(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<string> Enviar(string nome, string contentType, MemoryStream stream) {

            string extensao = Path.GetExtension(nome);

            string uuid = $"temp/{Guid.NewGuid()}{extensao}";

            await _storageClient.UploadObjectAsync(BUCKET_NAME, uuid, contentType, stream);

            return uuid;
        }

        internal async Task<byte[]> Download(string arquivo)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(BUCKET_NAME, arquivo, memoryStream);
            return memoryStream.ToArray();
        }
    }
}
