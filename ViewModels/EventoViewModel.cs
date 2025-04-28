using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class EventoViewModel
    {
        [Required]
        public string Nome { get; set; } = null!;

        [Required]
        public string Descricao { get; set; } = null!;

        [Required]
        public DateTime DataEvento { get; set; }

        public IFormFile? Imagem { get; set; }
    }
}