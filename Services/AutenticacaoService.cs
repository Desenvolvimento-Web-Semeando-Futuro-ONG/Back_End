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
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> Login(LoginViewModel loginVM)
        {
            var adm = await _context.Adms.FirstOrDefaultAsync(a => a.Login == loginVM.Login);
            if (adm == null || !adm.Autenticar(loginVM.Senha)) return null;

            return GerarTokenJwt(adm);
        }

        public string GerarTokenJwt(Adm adm)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, adm.Nome),
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