using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace boletim.Servicos
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            // 1. Pega as configurações do appsettings.json
            var secretKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing");
            var audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is missing");
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "3600");

            // 2. Prepara a chave de assinatura simétrica (usando HMAC SHA-256)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Monta o corpo do Token (Payload) com as Claims (incluindo as Roles)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes), // Tempo curto de vida do Access Token
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            // 4. Gera e serializa o JWT para uma string
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            // Gera um token opaco, criptograficamente seguro e aleatório para o Refresh Token
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            // Remove caracteres que poderiam quebrar URLs caso necessário
            return Convert.ToBase64String(randomNumber)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }
    }
}
