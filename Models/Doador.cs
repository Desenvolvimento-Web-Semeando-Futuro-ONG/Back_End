using Back_End.Enums;

namespace Back_End.Models
{
    public class Doador : Usuario
    {
        public List<Doacao> Doacoes { get; set; } = new();
    }
}