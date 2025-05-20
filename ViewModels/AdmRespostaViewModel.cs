using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class AdmRespostaViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = null!;

        public string? Telefone { get; set; }

        public string? CPF { get; set; }

        public string? Email { get; set; }

        [Required]
        public string Login { get; set; } = null!;
    }
}