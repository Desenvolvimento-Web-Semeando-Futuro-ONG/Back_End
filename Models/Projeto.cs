using Back_End.Enums;
using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class Projeto
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(1000)]
        public string Descricao { get; set; } = null!;

        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        [Required]
        public StatusProjeto Status { get; set; } = StatusProjeto.Ativo;

        public bool EhEventoEspecifico { get; set; } = false;
        public string? TipoEventoEspecifico { get; set; }

        public int CriadoPorAdmId { get; set; }
        public Adm? CriadoPorAdm { get; set; }

        public List<ProjetoVoluntario> Voluntarios { get; set; } = new();
    }
}