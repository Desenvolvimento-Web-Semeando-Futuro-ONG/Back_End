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

        //[HttpPost("galeria")]
        //public async Task<IActionResult> AdicionarFotoGaleria(IFormFile foto)
        //{
        //   var fotoId = await _admService.AdicionarFotoGaleria(foto);
        //   return Ok(new { id = fotoId });
        //}

        [HttpPost]
        //[AllowAnonymous] // Temporário para criação do primeiro admin
        public async Task<IActionResult> CriarAdm([FromBody] AdmViewModel admVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var admId = await _admService.CriarAdm(admVM);
                return Ok(new { id = admId, message = "Administrador criado com sucesso" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log do erro (implemente seu sistema de logging)
                Console.WriteLine($"Erro ao criar administrador: {ex.Message}");
                return StatusCode(500, new { message = "Ocorreu um erro interno ao criar o administrador" });
            }
        }

        [HttpGet("perfil")]
        [Authorize(Roles = "Adm")]
        [ProducesResponseType(typeof(AdmRespostaViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPerfil()
        {
            var admId = int.Parse(User.FindFirst("id").Value);
            var perfil = await _admService.ObterPerfilAdm(admId);

            return perfil == null ? NotFound() : Ok(perfil);
        }

        [HttpGet]
        [Authorize(Roles = "Adm")]
        [ProducesResponseType(typeof(List<AdmRespostaViewModel>), 200)]
        public async Task<IActionResult> ListarAdms()
        {
            var adms = await _admService.ListarAdms();
            return Ok(adms);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarAdm(int id, [FromBody] EditarAdmViewModel admVM)
        {
            try
            {
                var resultado = await _admService.EditarAdm(id, admVM);
                if (!resultado) return NotFound();
                return Ok(new { message = "Administrador atualizado com sucesso" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

            //[HttpDelete("galeria/{fotoId}")]
            //public async Task<IActionResult> RemoverFotoGaleria(string fotoId)
            //{
            //   var result = await _admService.RemoverFotoGaleria(fotoId);
            //   if (!result) return NotFound();
            //   return NoContent();
            //}

            //[HttpPost("publicacoes")]
            //public async Task<IActionResult> PublicarTexto([FromBody] string texto)
            //{
            //    var result = await _admService.PublicarTexto(texto);
            //   return Ok(new { message = result });
            //}
        }
}