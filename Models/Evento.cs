using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(100000)]
        public string Descricao { get; set; } = null!;

        [Required]
        public DateTime DataEvento { get; set; }

        public string? ImagemUrl { get; set; }

        public int CriadoPorAdmId { get; set; }
        public Adm? CriadoPorAdm { get; set; }

        public bool EhRascunho { get; set; } = true;

        public List<EventoVoluntario> Voluntarios { get; set; } = new();
    }
}