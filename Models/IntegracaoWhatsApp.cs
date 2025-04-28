using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class IntegracaoWhatsApp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Mensagem { get; set; } = null!;

        [Required]
        public string Destinatario { get; set; } = null!;

        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;
        public bool Enviado { get; set; } = false;
    }
}