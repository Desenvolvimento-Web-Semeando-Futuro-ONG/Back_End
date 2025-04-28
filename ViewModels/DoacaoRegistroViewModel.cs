using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class DoacaoRegistroViewModel
    {
        [Required]
        public decimal Valor { get; set; }

        [Required]
        public string MetodoPagamento { get; set; } = null!;
    }
}