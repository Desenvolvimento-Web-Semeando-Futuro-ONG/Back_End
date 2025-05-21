using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Back_End.Services.Interfaces;
using Back_End.ViewModels;
using Back_End.Models;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _projetoService;

        public ProjetoController(IProjetoService projetoService)
        {
            _projetoService = projetoService;
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
        [HttpPut("desativar/{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.DesativarProjetoAdmin(id, admId);
            if (!resultado) return NotFound();
            return NoContent();
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
                return NotFound(new { sucesso = false, mensagem = "Projeto não encontrado ou você não tem permissão" });

            return Ok(new
            {
                sucesso = true,
                mensagem = "Projeto ativado com sucesso",
                projeto = new
                {
                    projeto.Id,
                    projeto.Nome,
                    projeto.Status
                }
            });
        }

        [Authorize(Roles = "Voluntario")]
        [HttpDelete("{projetoId}/cancelar")]
        public async Task<IActionResult> CancelarInscricao(int projetoId)
        {
            var voluntarioId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.CancelarInscricao(projetoId, voluntarioId);
            if (!resultado) return BadRequest();
            return Ok(new { sucesso = true, mensagem = "Inscrição cancelada com sucesso" });
        }

        // ADMs gerenciando voluntários
        [Authorize(Roles = "Adm")]
        [HttpPut("{projetoId}/voluntarios/{voluntarioId}/aprovar")]
        public async Task<IActionResult> AprovarVoluntario(int projetoId, int voluntarioId)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.AprovarVoluntario(projetoId, voluntarioId, admId);
            if (!resultado) return BadRequest();
            return Ok(new { sucesso = true, mensagem = "Voluntário aprovado com sucesso" });
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("{projetoId}/voluntarios/{voluntarioId}/rejeitar")]
        public async Task<IActionResult> RejeitarVoluntario(int projetoId, int voluntarioId)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.RejeitarVoluntario(projetoId, voluntarioId, admId);
            if (!resultado) return BadRequest();
            return Ok(new { sucesso = true, mensagem = "Voluntário rejeitado com sucesso" });
        }

        [Authorize(Roles = "Adm")]
        [HttpGet("{projetoId}/voluntarios")]
        public async Task<IActionResult> ListarVoluntarios(int projetoId)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var voluntarios = await _projetoService.ListarVoluntariosPorProjeto(projetoId, admId);
            return Ok(voluntarios);
        }
    }
}
