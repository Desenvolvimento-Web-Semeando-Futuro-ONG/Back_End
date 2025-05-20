using Back_End.Services;
using Back_End.Services.Interfaces;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly GaleriaService _galeriaService;

        public EventoController(IEventoService eventoService, GaleriaService galeriaService)
        {
            _eventoService = eventoService;
            _galeriaService = galeriaService;
        }

        [Authorize(Roles = "Adm")]
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var eventos = await _eventoService.ListarTodos();
            return Ok(eventos);
        }

        [Authorize(Roles = "Adm")]
        [HttpGet("rascunhos")]
        public async Task<IActionResult> ListarRascunhos()
        {
            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var rascunhos = await _eventoService.ListarRascunhos(admId);
            return Ok(rascunhos);
        }

        [HttpGet("publicados")]
        public async Task<IActionResult> ListarPublicados()
        {
            var eventos = await _eventoService.ListarPublicados();
            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var evento = await _eventoService.ObterPorId(id);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [Authorize(Roles = "Adm")]
        [HttpPost]
        public async Task<IActionResult> Criar([FromForm] EventoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var admId = int.Parse(User.FindFirst("id")?.Value!);
            var evento = await _eventoService.Criar(model, admId, _galeriaService);
            return CreatedAtAction(nameof(ObterPorId), new { id = evento.Id }, evento);
        }

        [Authorize(Roles = "Adm")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromForm] EventoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var evento = await _eventoService.Atualizar(id, model, _galeriaService);
            if (evento == null) return NotFound();
            return Ok(evento);
        }

        [Authorize(Roles = "Adm")]
        [HttpPost("publicar/{id}")]
        public async Task<IActionResult> PublicarRascunho(int id)
        {
            var resultado = await _eventoService.PublicarRascunho(id);
            if (!resultado) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "Adm")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var resultado = await _eventoService.Deletar(id);
            if (!resultado) return NotFound();
            return NoContent();
        }

        [HttpGet("imagem/{id}")]
        public async Task<IActionResult> ObterImagemEvento(string id)
        {
            try
            {
                var imagem = await _galeriaService.ObterImagem(id);

                if (imagem == null)
                    return NotFound("Imagem não encontrada");

                return File(imagem.Dados, imagem.ContentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao recuperar a imagem: {ex.Message}");
            }
        }
    }
}