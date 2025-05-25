using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class DoadorViewModel
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(20)]
        public string Telefone { get; set; } = null!;

        [Required, StringLength(11)]
        public string CPF { get; set; } = null!;

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; } = null!;

        [Range(1.0, double.MaxValue, ErrorMessage = "O valor da doação deve ser maior que zero")]
        public decimal ValorDoacao { get; set; }

        [Required]
        public string MetodoPagamento { get; set; } = null!;
    }
}