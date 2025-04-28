using Back_End.Services.Interfaces;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoadorController : ControllerBase
    {
        private readonly IDoadorService _doadorService;

        public DoadorController(IDoadorService doadorService)
        {
            _doadorService = doadorService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] DoadorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se já existe cadastro com este CPF
            var existente = await _doadorService.ObterPorCpf(model.CPF);
            if (existente != null)
                return Ok(new { message = "Doador já cadastrado", doador = existente });

            var doador = await _doadorService.Cadastrar(model);
            return CreatedAtAction(nameof(ObterPorCpf), new { cpf = doador.CPF }, doador);
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            var doador = await _doadorService.ObterPorCpf(cpf);
            if (doador == null)
                return NotFound();

            return Ok(doador);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var doadores = await _doadorService.ListarTodos();
            return Ok(doadores);
        }

        [HttpPost("{doadorId}/doacoes")]
        public async Task<IActionResult> RegistrarDoacao(int doadorId, [FromBody] DoacaoRegistroViewModel model)
        {
            var doacao = await _doadorService.RegistrarDoacao(doadorId, model.Valor, model.MetodoPagamento);
            return Ok(doacao);
        }
    }
}