using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Back_End.Enums;

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

        [JsonIgnore]
        public virtual Adm CriadoPorAdm { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<ProjetoVoluntario> Voluntarios { get; set; } = new List<ProjetoVoluntario>();
    }
}