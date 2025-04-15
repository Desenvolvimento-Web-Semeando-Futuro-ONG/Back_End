using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        public Usuario() { }

        public Usuario(string nome, string telefone, string cpf, string email, string tipo)
        {
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
            Email = email;
            Tipo = tipo;
        }
    }
}
