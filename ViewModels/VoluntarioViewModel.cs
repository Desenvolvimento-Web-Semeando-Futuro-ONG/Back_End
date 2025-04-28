using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class VoluntarioViewModel
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(20)]
        public string Telefone { get; set; } = null!;

        [Required, StringLength(11)]
        public string CPF { get; set; } = null!;

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(500)]
        public string Habilidades { get; set; } = null!;

        [Required, StringLength(500)]
        public string Disponibilidade { get; set; } = null!;
    }
}