using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class CadastroComInscricaoViewModel
    {
        public int? ProjetoId { get; set; } 

        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(11)]
        public string CPF { get; set; } = null!;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = null!;

        [Required, StringLength(20)]
        public string Telefone { get; set; } = null!;

        [Required, StringLength(500)]
        public string Habilidades { get; set; } = null!;
        
        public string? FuncaoDesejada { get; set; }

        [Required, StringLength(500)]
        public string Disponibilidade { get; set; } = null!;
    }
}