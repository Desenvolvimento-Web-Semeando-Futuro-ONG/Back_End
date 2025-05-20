using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class AdmViewModel
    {
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [StringLength(100, ErrorMessage = "O nome deve ter no m�ximo 100 caracteres")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O telefone � obrigat�rio")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no m�ximo 20 caracteres")]
        public string Telefone { get; set; } = null!;

        [Required(ErrorMessage = "O CPF � obrigat�rio")]
        [StringLength(11, ErrorMessage = "CPF deve conter 11 d�gitos")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas n�meros")]
        public string CPF { get; set; } = null!;

        [Required(ErrorMessage = "O email � obrigat�rio")]
        [StringLength(100, ErrorMessage = "O email deve ter no m�ximo 100 caracteres")]
        [EmailAddress(ErrorMessage = "Email inv�lido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "O login � obrigat�rio")]
        [StringLength(50, ErrorMessage = "O login deve ter no m�ximo 50 caracteres")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres")]
        public string Senha { get; set; } = null!;
    }
}