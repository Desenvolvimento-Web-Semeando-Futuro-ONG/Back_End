namespace Back_End.Models
{
    public class ProjetoVoluntario
    {
        public int ProjetoId { get; set; }
        public Projeto? Projeto { get; set; }

        public int VoluntarioId { get; set; }
        public Voluntario? Voluntario { get; set; }

        public StatusInscricao Acao { get; set; }
        public DateTime DataInscricao { get; set; } = DateTime.Now;
        public StatusInscricao Status { get; set; } = StatusInscricao.Pendente;
        public string? FuncaoDesejada { get; set; }
    }
}

public enum StatusInscricao
{
    Pendente,
    Aprovado,
    Rejeitado,
    Concluido
}

public enum StatusProjeto
{
    Ativo,
    Inativo,
    Concluido
}