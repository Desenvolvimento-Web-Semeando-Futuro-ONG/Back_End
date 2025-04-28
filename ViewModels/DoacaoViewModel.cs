namespace Back_End.ViewModels
{
    public class DoacaoViewModel
    {
        public int Id { get; set; }
        public string DoadorNome { get; set; } = null!;
        public decimal Valor { get; set; }
        public DateTime DataDoacao { get; set; }
        public string MetodoPagamento { get; set; } = null!;
    }
}