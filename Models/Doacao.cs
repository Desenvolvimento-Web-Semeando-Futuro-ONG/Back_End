using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class Doacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public DateTime DataDoacao { get; set; }

        [Required]
        public string MetodoPagamento { get; set; } = null!;

        public int DoadorId { get; set; }
        public Doador? Doador { get; set; }

        public string? ComprovanteUrl { get; set; }
    }
}