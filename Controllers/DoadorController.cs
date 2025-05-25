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
        private readonly IAdmService _admService;

        public DoadorController(IDoadorService doadorService, IAdmService admService)
        {
            _doadorService = doadorService;
            _admService = admService;
        }

        [HttpGet("doacoes")]
        public async Task<IActionResult> ListarDoacoes()
        {
            var doacoes = await _admService.ListarDoacoes();
            return Ok(doacoes);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] DoadorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = await _doadorService.ObterPorCpf(model.CPF);
            if (existente != null)
                return Ok(new { message = "Doador já cadastrado", doador = existente });

            try
            {
                var doador = await _doadorService.Cadastrar(model);

                var response = new
                {
                    message = model.ValorDoacao > 0
                        ? "Doador cadastrado e doação registrada com sucesso"
                        : "Doador cadastrado com sucesso",
                    doador,
                    doacaoRegistrada = model.ValorDoacao > 0
                };

                return CreatedAtAction(nameof(ObterPorCpf), new { cpf = doador.CPF }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a solicitação", error = ex.Message });
            }
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var doacao = await _doadorService.RegistrarDoacao(
                    doadorId,
                    model.Valor,
                    model.MetodoPagamento);

                return Ok(new
                {
                    message = "Doação registrada com sucesso",
                    doacao = new
                    {
                        doacao.Id,
                        doacao.Valor,
                        doacao.DataDoacao,
                        doacao.MetodoPagamento
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao registrar a doação" });
            }
        }
    }
}