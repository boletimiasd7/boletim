namespace boletim.Models.Requests
{
    public class GoogleLoginRequest
    {
        /// <summary>
        /// O authorization code de uso único que o Google retornou na URL do Vue.
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// O segredo original do PKCE que o Vue gerou e enviou para cá.
        /// O .NET precisa disso para trocar o Code pelo Token final lá no Google.
        /// </summary>
        public required string CodeVerifier { get; set; }

        /// <summary>
        /// O Google exige a exata mesma URI que foi usada para iniciar o fluxo no front-end
        /// como medida extra de segurança contra falsificação de requisição.
        /// </summary>
        public required string RedirectUri { get; set; }
    }
}
