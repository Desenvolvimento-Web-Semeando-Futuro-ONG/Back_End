using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class DoacaoRegistroViewModel
    {
        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required]
        public string MetodoPagamento { get; set; } = null!;
    }
}