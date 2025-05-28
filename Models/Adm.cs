using Back_End.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace Back_End.Models
{
    public class Adm : Usuario
    {
        [Required, StringLength(50)]
        public string Login { get; set; } = null!;

        [Required]
        public string SenhaHash { get; set; } = null!;

        public Adm()
        {
            Tipo = TipoUsuario.Adm;
        }

        public void DefinirSenha(string senha)
        {
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool Autenticar(string senha)
        {
            if (this.SenhaHash != null && this.SenhaHash.Length == 44 && this.SenhaHash.EndsWith("="))
            {
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                var hash = Convert.ToBase64String(bytes);

                if (hash == this.SenhaHash)
                {
                    this.DefinirSenha(senha);
                    return true;
                }
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(senha, this.SenhaHash);
        }

    }
}