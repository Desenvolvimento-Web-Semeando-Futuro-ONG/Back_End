using Back_End.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Back_End.Data;
using Back_End;
using Back_End.Models;
using Back_End.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoluntarioController : ControllerBase
    {
        private readonly IVoluntarioService _voluntarioService;
        private readonly AppDbContext _context;
        private readonly IAutenticacaoService _authService;

        public VoluntarioController(AppDbContext context, IAutenticacaoService authService, IVoluntarioService voluntarioService)
        {
            _context = context;
            _authService = authService;
            _voluntarioService = voluntarioService;
        }

        [HttpPost("cadastrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar([FromBody] CadastroComInscricaoViewModel model)
        {
            // 1. Validação
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Cria/atualiza voluntário
            var voluntario = await _context.Voluntarios
                .FirstOrDefaultAsync(v => v.CPF == model.CPF);

            if (voluntario == null)
            {
                voluntario = new Voluntario
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    Email = model.Email,
                    Telefone = model.Telefone,
                    Habilidades = model.Habilidades,
                    Disponibilidade = model.Disponibilidade
                };
                _context.Voluntarios.Add(voluntario);
            }
            else
            {
                // Atualiza dados existentes
                voluntario.Nome = model.Nome;
                voluntario.Email = model.Email;
                voluntario.Telefone = model.Telefone;
            }

            // 3. Inscreve no projeto (se houver projetoId)
            if (model.ProjetoId.HasValue)
            {
                var jaInscrito = await _context.ProjetoVoluntarios
                    .AnyAsync(pv => pv.ProjetoId == model.ProjetoId &&
                                   pv.VoluntarioId == voluntario.Id);

                if (!jaInscrito)
                {
                    _context.ProjetoVoluntarios.Add(new ProjetoVoluntario
                    {
                        ProjetoId = model.ProjetoId.Value,
                        VoluntarioId = voluntario.Id,
                        Status = StatusInscricao.Pendente,
                        DataInscricao = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();

            // 4. Gera token (opcional)
            var token = _authService.GerarTokenVoluntario(voluntario);

            // 5. Retorna resposta unificada
            return Ok(new
            {
                Sucesso = true,
                Mensagem = voluntario.Id == 0 ?
                    "Cadastro realizado!" :
                    "Dados atualizados com sucesso",
                Token = token, 
                VoluntarioId = voluntario.Id,
                ProjetoId = model.ProjetoId
            });
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            var voluntario = await _voluntarioService.ObterPorCpf(cpf);
            if (voluntario == null)
                return NotFound();

            return Ok(voluntario);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var voluntarios = await _voluntarioService.ListarTodos();
            return Ok(voluntarios);
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> ListarPorEvento(int eventoId)
        {
            var voluntarios = await _voluntarioService.ListarPorEvento(eventoId);
            return Ok(voluntarios);
        }
    }
}