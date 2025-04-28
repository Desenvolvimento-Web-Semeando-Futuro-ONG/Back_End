using Back_End.Enums;
using System.ComponentModel.DataAnnotations;

namespace Back_End.Models
{
    public class Voluntario : Usuario
    {
        [Required, StringLength(500)]
        public string Habilidades { get; set; } = null!;

        [Required, StringLength(500)]
        public string Disponibilidade { get; set; } = null!;

        public List<EventoVoluntario> Eventos { get; set; } = new();
    }
}