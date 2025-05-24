using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back_End.Models
{
    public class HistoricoAprovacao
    {
        [Key]
        public int Id { get; set; }

        public int ProjetoId { get; set; }
        public int VoluntarioId { get; set; }
        public int AdministradorId { get; set; }
        public StatusInscricao Acao { get; set; }
        public DateTime DataAcao { get; set; }
        public string? Observacao { get; set; }

        [JsonIgnore]
        public virtual Projeto Projeto { get; set; } = null!;

        [JsonIgnore]
        public virtual Voluntario Voluntario { get; set; } = null!;

        [JsonIgnore]
        public virtual Adm Administrador { get; set; } = null!;
    }
}