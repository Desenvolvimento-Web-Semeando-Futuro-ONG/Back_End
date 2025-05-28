using System.ComponentModel.DataAnnotations;
using Back_End.Enums;

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

        [Required, StringLength(11), RegularExpression(@"^\d{11}$")]
        public string CPF { get; set; } = null!;

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; } = null!;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public TipoUsuario Tipo { get; internal set; }

        public void DefinirTipo(TipoUsuario tipo)
        {
            Tipo = tipo;
        }
    }
}