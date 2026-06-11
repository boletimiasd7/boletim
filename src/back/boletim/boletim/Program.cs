using boletim.Database;
using boletim.Servicos;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BoletimDb>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ArquivoServico>();
builder.Services.AddSingleton(provider => StorageClient.Create());
builder.Services.AddHttpClient<IGoogleAuthClient, GoogleAuthClient>();
builder.Services.AddScoped<IJwtService, JwtService>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// 2. Avisa o .NET que o padrão de autenticação será o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Força o uso de HTTPS
    options.SaveToken = true; // Salva o token no HttpContext para acesso posterior, se necessário

    // 3. Define as regras de validação (A "porta giratória" do seu back-end)
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Exige que a chave secreta bata
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

        // Exige que o emissor e o público sejam os que definimos
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,

        // Valida se o token não expirou (Regra vital de segurança)
        ValidateLifetime = true,

        // O .NET dá 5 minutos de tolerância por padrão para tokens expirados.
        // Zeramos isso para o Refresh Token do Vue entrar em ação imediatamente.
        ClockSkew = TimeSpan.Zero,

        // Avisa ao .NET qual Claim contém as Roles para o [Authorize(Roles = "...")] funcionar
        RoleClaimType = ClaimTypes.Role,
    };
});

// 4. Habilita o serviço de Autorização (Roles e Policies)
builder.Services.AddAuthorization();

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
