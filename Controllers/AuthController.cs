using Back_End.Data;
using Back_End.Services.Interfaces;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAutenticacaoService _authService;

        public AuthController(AppDbContext context, IAutenticacaoService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginVM)
        {
            if (loginVM == null)
                return BadRequest("Dados inválidos");

            var adm = await _context.Adms.FirstOrDefaultAsync(a => a.Login == loginVM.Login);
            if (adm == null)
                return Unauthorized("Usuário não encontrado");

            if (adm.Autenticar(loginVM.Senha))
            {
                await _context.SaveChangesAsync();
                var token = _authService.GerarTokenJwt(adm);
                return Ok(new { token });
            }

            return Unauthorized("Credenciais inválidas");
        }
    }
}