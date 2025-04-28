using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class Publicacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Texto { get; set; } = null!;

        public DateTime DataPublicacao { get; set; }
        public int AdmId { get; set; }
        public Adm? Adm { get; set; }
    }
}