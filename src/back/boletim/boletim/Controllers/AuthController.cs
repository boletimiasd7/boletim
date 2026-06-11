using boletim.Models.Requests;
using boletim.Servicos;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace boletim.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IGoogleAuthClient _googleAuthClient;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(IGoogleAuthClient googleAuthClient, IJwtService jwtService, IConfiguration configuration)
        {
            _googleAuthClient = googleAuthClient;
            _jwtService = jwtService;
            _configuration = configuration;
        }


        [HttpPost("google-login")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin(GoogleLoginRequest request)
        {
            try
            {
                // 1. O .NET faz o POST para https://oauth2.googleapis.com/token
                // Enviando: client_id, client_secret, request.Code, request.CodeVerifier e grant_type=authorization_code

                var googleTokens = await _googleAuthClient.ExchangeCodeForTokensAsync(request);
                
                var clientId = _configuration["Authentication:Google:ClientId"];

                // 2. Extrai o Email do id_token retornado pelo Google
                var userEmail = await JwtDecoder.GetEmailAsync(googleTokens.IdToken, clientId!);

                // 3. Busca o usuário no seu Banco de Dados pelo email
                var user = new { Id = 1, Email = "fabbiolimmadasilva@gmail.com", Role = "Administrador" };
                if (user == null) return Unauthorized("Usuário não cadastrado.");

                // 4. Regra 4: Monta as Roles do usuário (Administrador, Comunicador, etc)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role) // Ex: "Administrador de Igreja"
                };

                // 5. Gera o seu próprio JWT e Refresh Token
                var accessToken = _jwtService.GenerateAccessToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // 6. Salva o Refresh Token no banco atrelado a este usuário para validação futura
                //await _userRepository.SaveRefreshTokenAsync(user.Id, refreshToken);

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous] // Regra 3: Liberado sem [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // 1. Valida se o Refresh Token do front-end existe no banco de dados e não expirou
            // 2. Gera um novo AccessToken e um novo RefreshToken
            // 3. Retorna para o Vue
            return Ok();
        }
    }
}