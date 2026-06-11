using boletim.Models.Requests;
using boletim.Models.Responses;

namespace boletim.Servicos
{


    public interface IGoogleAuthClient
    {
        Task<GoogleTokenResponse> ExchangeCodeForTokensAsync(GoogleLoginRequest request);
    }

    public class GoogleAuthClient : IGoogleAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _clientId = "37795619433-83l7apub5jc5igc3lvroh3oeru3gmrm1.apps.googleusercontent.com";
        private readonly string _clientSecret = "";

        public GoogleAuthClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<GoogleTokenResponse> ExchangeCodeForTokensAsync(GoogleLoginRequest request)
        {
            // Pega as credenciais seguras do seu appsettings.json ou User Secrets
            var clientId = _configuration["Authentication:Google:ClientId"];
            var clientSecret = _configuration["Authentication:Google:ClientSecret"];

            // Monta o formulário no formato x-www-form-urlencoded exigido pelo OAuth2
            var tokenRequestParameters = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", request.Code },
            { "code_verifier", request.CodeVerifier }, // O PKCE magic acontece aqui!
            { "grant_type", "authorization_code" },
            { "redirect_uri", request.RedirectUri }
        };

            var content = new FormUrlEncodedContent(tokenRequestParameters);

            // Faz o POST silencioso de Back-end para Back-end
            var response = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Falha ao trocar o código no Google. Detalhes: {error}");
            }

            // Converte o JSON do Google para a nossa classe DTO
            var googleTokens = await response.Content.ReadFromJsonAsync<GoogleTokenResponse>();

            return googleTokens ?? throw new Exception("Resposta do Google veio vazia.");
        }
    }
}
