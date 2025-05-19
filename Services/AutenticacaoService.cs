using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Back_End.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Back_End.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AutenticacaoService(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string?> LoginAdm(LoginViewModel loginVM)
        {
            if (loginVM == null || string.IsNullOrWhiteSpace(loginVM.Login))
            {
                return null;
            }

            var adm = await _context.Adms
                .FirstOrDefaultAsync(a => a.Login == loginVM.Login);

            if (adm == null || !adm.Autenticar(loginVM.Senha))
            {
                return null;
            }

            return GerarTokenAdm(adm);
        }

        public string GerarTokenVoluntario(Voluntario voluntario)
        {
            if (voluntario == null)
            {
                throw new ArgumentNullException(nameof(voluntario));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, voluntario.Nome ?? string.Empty),
                    new Claim(ClaimTypes.Role, "Voluntario"),
                    new Claim("id", voluntario.Id.ToString()),
                    new Claim("cpf", voluntario.CPF ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GerarTokenAdm(Adm adm)
        {
            if (adm == null)
            {
                throw new ArgumentNullException(nameof(adm));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, adm.Nome ?? string.Empty),
                    new Claim(ClaimTypes.Role, "Adm"),
                    new Claim("id", adm.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}