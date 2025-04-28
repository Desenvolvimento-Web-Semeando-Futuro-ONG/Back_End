using Back_End.Services.Interfaces;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoluntarioController : ControllerBase
    {
        private readonly IVoluntarioService _voluntarioService;

        public VoluntarioController(IVoluntarioService voluntarioService)
        {
            _voluntarioService = voluntarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] VoluntarioViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se já existe cadastro com este CPF
            var existente = await _voluntarioService.ObterPorCpf(model.CPF);
            if (existente != null)
                return Ok(new { message = "Voluntário já cadastrado", voluntario = existente });

            var voluntario = await _voluntarioService.Cadastrar(model);
            return CreatedAtAction(nameof(ObterPorCpf), new { cpf = voluntario.CPF }, voluntario);
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