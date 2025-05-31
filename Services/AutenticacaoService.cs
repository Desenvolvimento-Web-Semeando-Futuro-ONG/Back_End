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
                return null;

            var adm = await _context.Adms.FirstOrDefaultAsync(a => a.Login == loginVM.Login);
            return adm != null && adm.Autenticar(loginVM.Senha) ? GerarTokenAdm(adm) : null;
        }

        public string GerarTokenVoluntario(Voluntario voluntario)
        {
            if (voluntario == null)
                throw new ArgumentNullException(nameof(voluntario));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

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

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public string GerarTokenAdm(Adm adm)
        {
            if (adm == null)
                throw new ArgumentNullException(nameof(adm));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

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

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public async Task<bool> SolicitarRedefinicaoSenha(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var adm = await _context.Adms.FirstOrDefaultAsync(a => a.Email == email);
            if (adm == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", adm.Id.ToString()),
                    new Claim("action", "reset_password")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var resetLink = $"{_configuration["AppUrl"]}/reset-password?token={tokenHandler.WriteToken(token)}";

            Console.WriteLine($"Link de redefinição para {adm.Email}: {resetLink}");
            return true;
        }

        public async Task<bool> RedefinirSenha(RedefinirSenhaViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Token) || string.IsNullOrWhiteSpace(model.NovaSenha))
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

                tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                var action = jwtToken.Claims.First(x => x.Type == "action").Value;

                if (action != "reset_password")
                    return false;

                var adm = await _context.Adms.FindAsync(int.Parse(userId));
                if (adm == null)
                    return false;

                adm.RedefinirSenha(model.NovaSenha);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}