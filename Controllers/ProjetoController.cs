using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Back_End.Services.Interfaces;
using Back_End.ViewModels;

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

        // P�blico
        [HttpGet("ativos")]
        public async Task<IActionResult> ListarAtivos()
        {
            var projetos = await _projetoService.ListarProjetosAtivos();
            return Ok(projetos);
        }

        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> ListarPorTipo(string tipo)
        {
            var projetos = await _projetoService.ListarProjetosPorTipo(tipo);
            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var projeto = await _projetoService.ObterProjetoPorId(id);
            if (projeto == null) return NotFound();
            return Ok(projeto);
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

        // Volunt�rios
        [Authorize(Roles = "Voluntario")]
        [HttpPost("{projetoId}/inscrever")]
        public async Task<IActionResult> Inscrever(int projetoId)
        {
            var voluntarioId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.InscreverVoluntario(projetoId, voluntarioId);
            if (!resultado) return BadRequest();
            return Ok();
        }

        [Authorize(Roles = "Voluntario")]
        [HttpDelete("{projetoId}/cancelar")]
        public async Task<IActionResult> CancelarInscricao(int projetoId)
        {
            var voluntarioId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.CancelarInscricao(projetoId, voluntarioId);
            if (!resultado) return BadRequest();
            return Ok();
        }

        // ADMs gerenciando volunt�rios
        [Authorize(Roles = "Adm")]
        [HttpPut("{projetoId}/voluntarios/{voluntarioId}/aprovar")]
        public async Task<IActionResult> AprovarVoluntario(int projetoId, int voluntarioId)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.AprovarVoluntario(projetoId, voluntarioId, admId);
            if (!resultado) return BadRequest();
            return Ok();
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("{projetoId}/voluntarios/{voluntarioId}/rejeitar")]
        public async Task<IActionResult> RejeitarVoluntario(int projetoId, int voluntarioId)
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var resultado = await _projetoService.RejeitarVoluntario(projetoId, voluntarioId, admId);
            if (!resultado) return BadRequest();
            return Ok();
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