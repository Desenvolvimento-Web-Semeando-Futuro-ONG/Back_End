using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Back_End.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Back_End.ViewModels;
using Back_End.Models;
using Back_End.Data;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _projetoService;
        private readonly AppDbContext _context; 

        public ProjetoController(IProjetoService projetoService, AppDbContext context) 
        {
            _projetoService = projetoService;
            _context = context;
        }

        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos()
        {
            var projetos = await _projetoService.ListarProjetosAtivos();
            return Ok(projetos.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                p.DataInicio,
                p.DataFim,
                p.Status,
                p.EhEventoEspecifico,
                p.TipoEventoEspecifico,
                VoluntariosCount = p.Voluntarios.Count
            }));
        }

        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> ListarPorTipo(string tipo)
        {
            var projetos = await _projetoService.ListarProjetosPorTipo(tipo);
            return Ok(projetos.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                p.DataInicio,
                p.Status
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var projeto = await _projetoService.ObterProjetoPorId(id);
            if (projeto == null) return NotFound();

            return Ok(new
            {
                projeto.Id,
                projeto.Nome,
                projeto.Descricao,
                projeto.DataInicio,
                projeto.DataFim,
                projeto.Status,
                projeto.EhEventoEspecifico,
                projeto.TipoEventoEspecifico,
                Voluntarios = projeto.Voluntarios.Select(v => new
                {
                    v.VoluntarioId,
                    v.Voluntario?.Nome,
                    v.Status
                })
            });
        }

        [HttpPost("inscrever")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Inscrever([FromBody] CadastroComInscricaoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await _projetoService.CadastrarVoluntarioComInscricao(model);
        }

        // Apenas ADMs
        [Authorize(Roles = "Adm")]
        [HttpGet("admin")]
        public async Task<IActionResult> ListarTodosAdmin()
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projetos = await _projetoService.ListarTodosProjetosAdmin(admId);
            return Ok(projetos);
        }

        [Authorize(Roles = "Adm")]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ProjetoViewModel model)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projeto = await _projetoService.CriarProjetoAdmin(model, admId);
            return CreatedAtAction(nameof(ObterPorId), new { id = projeto.Id }, projeto);
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ProjetoViewModel model)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projeto = await _projetoService.AtualizarProjetoAdmin(id, model, admId);
            if (projeto == null) return NotFound();
            return Ok(projeto);
        }

        [Authorize(Roles = "Adm")]
        [HttpGet("desativados")]
        public async Task<IActionResult> ListarProjetosDesativados()
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projetos = await _projetoService.ListarProjetosDesativados(admId);

            return Ok(projetos.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                p.DataInicio,
                p.DataFim,
                p.Status,
                CriadoPor = p.CriadoPorAdm.Nome
            }));
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarProjeto(int id)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projeto = await _projetoService.AtivarProjeto(id, admId);

            if (projeto == null)
                return NotFound(new { sucesso = false, mensagem = "Projeto n�o encontrado" });

            return Ok(new
            {
                sucesso = true,
                mensagem = "Projeto ativado com sucesso",
                status = projeto.Status.ToString()
            });
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("desativar/{id}")]
        public async Task<IActionResult> DesativarProjeto(int id)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projeto = await _projetoService.DesativarProjeto(id, admId);

            if (projeto == null)
                return NotFound(new { sucesso = false, mensagem = "Projeto n�o encontrado" });

            return Ok(new
            {
                sucesso = true,
                mensagem = "Projeto desativado com sucesso",
                status = projeto.Status.ToString()
            });
        }

        [Authorize(Roles = "Voluntario")]
        [HttpDelete("{projetoId}/cancelar")]
        public async Task<IActionResult> CancelarInscricao(int projetoId)
        {
            var voluntarioId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.CancelarInscricao(projetoId, voluntarioId);

            if (!resultado)
                return BadRequest(new { sucesso = false, mensagem = "Falha ao cancelar inscri��o" });

            return Ok(new { sucesso = true, mensagem = "Inscri��o cancelada com sucesso" });
        }

        [Authorize(Roles = "Adm")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProjeto(int id)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.DeletarProjeto(id, admId);

            if (!resultado)
                return BadRequest(new { sucesso = false, mensagem = "Falha ao deletar projeto" });

            return Ok(new { sucesso = true, mensagem = "Projeto deletado com sucesso" });
        }

        // Adicione no ProjetoController
        [HttpGet("estatisticas/total-inscritos")]
        public async Task<IActionResult> ObterTotalInscritosPorProjeto()
        {
            var resultado = await _projetoService.ObterTotalInscritosPorProjeto();
            return Ok(resultado);
        }

        [HttpGet("estatisticas/total-geral-inscritos")]
        public async Task<IActionResult> ObterTotalGeralInscritos()
        {
            var total = await _projetoService.ObterTotalGeralInscritos();
            return Ok(new { TotalInscritos = total });
        }

        [HttpGet("estatisticas/projeto-menos-escolhido")]
        public async Task<IActionResult> ObterProjetoMenosEscolhido()
        {
            var projeto = await _projetoService.ObterProjetoMenosEscolhido();
            if (projeto == null) return NotFound();

            return Ok(new
            {
                projeto.Id,
                projeto.Nome,
                TotalInscritos = projeto.Voluntarios.Count(v => v.Status == StatusInscricao.Aprovado)
            });
        }

        [HttpGet("estatisticas/atividade-mais-escolhida")]
        public async Task<IActionResult> ObterAtividadeMaisEscolhida()
        {
            var atividade = await _projetoService.ObterAtividadeMaisEscolhida();
            return Ok(new { AtividadeMaisEscolhida = atividade });
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("{projetoId}/voluntarios/{voluntarioId}/aprovar")]
        public async Task<IActionResult> AprovarVoluntario(int projetoId, int voluntarioId, [FromBody] string? observacao = null)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.AprovarVoluntario(projetoId, voluntarioId, admId, observacao);
            if (!resultado) return BadRequest();
            return Ok(new { sucesso = true, mensagem = "Volunt�rio aprovado com sucesso" });
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("{projetoId}/voluntarios/{voluntarioId}/rejeitar")]
        public async Task<IActionResult> RejeitarVoluntario(int projetoId, int voluntarioId, [FromBody] string? observacao = null)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.RejeitarVoluntario(projetoId, voluntarioId, admId, observacao);
            if (!resultado) return BadRequest();
            return Ok(new { sucesso = true, mensagem = "Volunt�rio rejeitado com sucesso" });
        }

        [Authorize(Roles = "Adm")]
        [HttpGet("{projetoId}/voluntarios")]
        public async Task<IActionResult> ListarVoluntarios(int projetoId, [FromQuery] StatusInscricao? status = null)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == projetoId && p.CriadoPorAdmId == admId);

            if (projeto == null) return NotFound();

            var query = _context.ProjetoVoluntarios
                .Where(pv => pv.ProjetoId == projetoId)
                .Include(pv => pv.Voluntario)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(pv => pv.Status == status.Value);
            }

            var voluntarios = await query
                .Select(pv => new
                {
                    pv.VoluntarioId,
                    pv.Voluntario.Nome,
                    pv.Voluntario.Email,
                    pv.Status,
                    pv.DataInscricao,
                    pv.FuncaoDesejada
                })
                .ToListAsync();

            return Ok(voluntarios);
        }
    }
}
