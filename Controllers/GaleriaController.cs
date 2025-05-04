using Microsoft.AspNetCore.Mvc;
using Back_End.Services;
using MongoDB.Bson;
using System.Net.Mime;

namespace Back_End.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GaleriaController : ControllerBase
	{
		private readonly GaleriaService _galeriaService;

		public GaleriaController(GaleriaService galeriaService)
		{
			_galeriaService = galeriaService;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadImagem(IFormFile imagem)
		{
			try
			{
				if (imagem == null || imagem.Length == 0)
					return BadRequest("Nenhuma imagem foi enviada");

				if (!imagem.ContentType.StartsWith("image/"))
					return BadRequest("O arquivo enviado não é uma imagem válida");

				var id = await _galeriaService.UploadImagem(imagem);
				return Ok(new { id });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Erro interno ao processar a imagem: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> ObterImagem(string id)
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

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletarImagem(string id)
		{
			try
			{
				var sucesso = await _galeriaService.DeletarImagem(id);
				return sucesso ? NoContent() : NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Erro interno ao excluir a imagem: {ex.Message}");
			}
		}
	}
}