using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back_End.Models
{
    public class Voluntario : Usuario
    {
        [Required, StringLength(500)]
        public string Habilidades { get; set; } = null!;

        [Required, StringLength(500)]
        public string Disponibilidade { get; set; } = null!;

        [JsonIgnore]
        public List<EventoVoluntario> Eventos { get; set; } = new();

        [JsonIgnore]
        public List<ProjetoVoluntario> Projetos { get; set; } = new();
    }
}