using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
	public class EditarAdmViewModel
	{
		public string? Nome { get; set; }
		public string? Login { get; set; }
		public string? Senha { get; set; }
	}
}