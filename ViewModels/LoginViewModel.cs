using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O login � obrigat�rio")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        public string Senha { get; set; } = null!;
    }

}