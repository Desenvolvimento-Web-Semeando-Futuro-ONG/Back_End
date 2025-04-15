using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using Back_End.Interfaces;

namespace Back_End.Models
{
    [Table("Adms")]
    public class Adm : Usuario, IAutenticacao
    {
        [Required, MaxLength(20)]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Senha { get; private set; } = string.Empty;

        public bool Autenticar(string senha)
        {
            return Senha == GerarHash(senha);
        }

        public void DefinirSenha(string senha)
        {
            Senha = GerarHash(senha);
        }

        private static string GerarHash(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
