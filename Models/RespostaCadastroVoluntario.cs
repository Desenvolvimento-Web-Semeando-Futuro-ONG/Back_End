namespace Back_End.Models
{
    public class RespostaCadastroVoluntario
    {
        public Voluntario Voluntario { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Mensagem { get; set; } = null!;
    }
}