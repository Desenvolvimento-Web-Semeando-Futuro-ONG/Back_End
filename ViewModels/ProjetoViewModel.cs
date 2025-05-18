using System.ComponentModel.DataAnnotations;

namespace Back_End.ViewModels
{
    public class ProjetoViewModel
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(1000)]
        public string Descricao { get; set; } = null!;

        [Required]
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public bool EhEventoEspecifico { get; set; } = false;
        public string? TipoEventoEspecifico { get; set; }
    }
}