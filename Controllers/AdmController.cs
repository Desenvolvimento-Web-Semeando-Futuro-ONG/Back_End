using Back_End.Services.Interfaces;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [Authorize(Roles = "Adm")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdmController : ControllerBase
    {
        private readonly IAdmService _admService;

        public AdmController(IAdmService admService)
        {
            _admService = admService;
        }


        [HttpPut("eventos/{eventoId}/voluntarios")]
        public async Task<IActionResult> EscalarVoluntarios(int eventoId, [FromBody] List<int> voluntariosIds)
        {
            var result = await _admService.EscalarVoluntarios(eventoId, voluntariosIds);
            if (!result) return NotFound();
            return Ok(new { message = "Voluntários escalados com sucesso" });
        }

        [HttpGet("doacoes")]
        public async Task<IActionResult> ListarDoacoes()
        {
            var doacoes = await _admService.ListarDoacoes();
            return Ok(doacoes);
        }

        [HttpPost("galeria")]
        public async Task<IActionResult> AdicionarFotoGaleria(IFormFile foto)
        {
            var fotoId = await _admService.AdicionarFotoGaleria(foto);
            return Ok(new { id = fotoId });
        }

        //[HttpDelete("galeria/{fotoId}")]
        //public async Task<IActionResult> RemoverFotoGaleria(string fotoId)
        //{
        //   var result = await _admService.RemoverFotoGaleria(fotoId);
        //   if (!result) return NotFound();
        //   return NoContent();
        //}

        [HttpPost("publicacoes")]
        public async Task<IActionResult> PublicarTexto([FromBody] string texto)
        {
            var result = await _admService.PublicarTexto(texto);
            return Ok(new { message = result });
        }
    }
}