using Back_End.Enums;

namespace Back_End.Models
{
    public class Doador : Usuario
    {
        public Doador()
        {
            Tipo = TipoUsuario.Doador;
        }
        public List<Doacao> Doacoes { get; set; } = new();
    }
}