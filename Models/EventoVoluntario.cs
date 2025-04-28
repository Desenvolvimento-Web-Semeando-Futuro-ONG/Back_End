namespace Back_End.Models
{
    public class EventoVoluntario
    {
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }

        public int VoluntarioId { get; set; }
        public Voluntario? Voluntario { get; set; }

        public DateTime DataInscricao { get; set; } = DateTime.UtcNow;
    }
}