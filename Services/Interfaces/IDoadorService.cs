using Back_End.Models;
using Back_End.ViewModels;

namespace Back_End.Services.Interfaces
{
    public interface IDoadorService
    {
        Task<Doador?> ObterPorCpf(string cpf);
        Task<Doador> Cadastrar(DoadorViewModel model);
        Task<List<Doador>> ListarTodos();
        Task<List<DoacaoViewModel>> ListarDoacoes();
        Task<Doacao> RegistrarDoacao(int doadorId, decimal valor, string metodoPagamento);
    }
}