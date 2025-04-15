using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Back_End.Models;
using Back_End.ViewModels;
using Back_End.Data;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdmController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdmController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("criar")]
        public IActionResult CriarAdm([FromBody] AdmViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adm = new Adm
            {
                Nome = model.Nome,
                Telefone = model.Telefone,
                CPF = model.CPF,
                Email = model.Email,
                Tipo = "Adm",
                Login = model.Login
            };

            adm.DefinirSenha(model.Senha);
            _context.Adms.Add(adm);
            _context.SaveChanges();

            return Ok(new { mensagem = "Administrador criado com sucesso!" });
        }

        [HttpGet]
        [Authorize(Roles = "Adm")]
        public IActionResult ListarAdms()
        {
            var adms = _context.Adms
                .Select(a => new
                {
                    a.Id,
                    a.Login,
                    a.Nome,
                    a.Email,
                    a.Telefone,
                    a.CPF
                })
                .ToList();

            return Ok(adms);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Adm")]
        public IActionResult BuscarAdmPorId(int id)
        {
            var adm = _context.Adms.FirstOrDefault(a => a.Id == id);

            if (adm == null)
                return NotFound(new { mensagem = "Administrador não encontrado." });

            return Ok(new
            {
                adm.Id,
                adm.Login,
                adm.Nome,
                adm.Email,
                adm.Telefone,
                adm.CPF
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Adm")]
        public IActionResult AtualizarAdm(int id, [FromBody] AdmViewModel model)
        {
            var adm = _context.Adms.FirstOrDefault(a => a.Id == id);

            if (adm == null)
                return NotFound(new { mensagem = "Administrador não encontrado." });

            adm.Login = model.Login;
            adm.DefinirSenha(model.Senha);
            adm.Nome = model.Nome;
            adm.Telefone = model.Telefone;
            adm.CPF = model.CPF;
            adm.Email = model.Email;

            _context.SaveChanges();

            return Ok(new { mensagem = "Administrador atualizado com sucesso!" });
        }

        [HttpPut("alterar-senha")]
        [Authorize(Roles = "Adm")]
        public IActionResult AlterarSenha([FromBody] AlterarSenhaViewModel model)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var adm = _context.Adms.Find(id);

            if (adm == null || !adm.Autenticar(model.SenhaAtual))
                return Unauthorized(new { mensagem = "Senha atual incorreta." });

            adm.DefinirSenha(model.NovaSenha);
            _context.SaveChanges();

            return Ok(new { mensagem = "Senha alterada com sucesso." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Adm")]
        public IActionResult DeletarAdm(int id)
        {
            var adm = _context.Adms.FirstOrDefault(a => a.Id == id);

            if (adm == null)
                return NotFound(new { mensagem = "Administrador não encontrado." });

            _context.Adms.Remove(adm);
            _context.SaveChanges();

            return Ok(new { mensagem = "Administrador removido com sucesso!" });
        }
    }
}
