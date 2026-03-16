namespace boletim.Servicos
{
    using Google.Apis.Auth;

    public static class JwtDecoder
    {
        /// <summary>
        /// Valida a assinatura do IdToken no Google e extrai o e-mail do usuário.
        /// </summary>
        /// <param name="idToken">O JWT gigante retornado pela API do Google.</param>
        /// <param name="clientId">O seu Client ID do Google Cloud Console.</param>
        /// <returns>O e-mail validado do usuário.</returns>
        public static async Task<string> GetEmailAsync(string idToken, string clientId)
        {
            try
            {
                // Configuramos o validador para exigir que o token tenha sido gerado
                // especificamente para o nosso aplicativo (Audience)
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { clientId }
                };

                // A MÁGICA ACONTECE AQUI:
                // Essa função baixa as chaves públicas do Google (em cache),
                // verifica a assinatura RSA, checa a data de expiração e valida o emissor.
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                if (string.IsNullOrEmpty(payload.Email))
                {
                    throw new Exception("O e-mail não foi fornecido pelo Google.");
                }

                return payload.Email;
            }
            catch (InvalidJwtException ex)
            {
                // Cai aqui se o token for falso, adulterado ou estiver expirado
                throw new Exception($"Assinatura do token do Google inválida: {ex.Message}");
            }
        }
    }
}
