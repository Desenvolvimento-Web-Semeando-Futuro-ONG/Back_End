using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Back_End.Models;
using Back_End.Data;
using Back_End.ViewModels;



namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var adm = _context.Adms.FirstOrDefault(a => a.Login == model.Login);

            if (adm == null || !adm.Autenticar(model.Senha))
                return Unauthorized(new { mensagem = "Login ou senha inválidos." });

            var token = GerarToken(adm);

            return Ok(new { token });
        }

        private string GerarToken(Adm adm)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, adm.Id.ToString()),
                new Claim(ClaimTypes.Role, "Adm")
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
