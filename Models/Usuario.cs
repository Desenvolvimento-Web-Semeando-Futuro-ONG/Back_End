using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public abstract class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(20)]
        public string Telefone { get; set; } = null!;

        [Required, StringLength(11), RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos")]
        public string CPF { get; set; } = null!;

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; } = null!;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}