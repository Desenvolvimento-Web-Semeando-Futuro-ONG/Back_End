using Back_End.Models;
using Back_End.ViewModels;

namespace Back_End.Services.Interfaces
{
    public interface IVoluntarioService
    {
        Task<Voluntario?> ObterPorCpf(string cpf);
        Task<Voluntario> Cadastrar(VoluntarioViewModel model);
        Task<List<Voluntario>> ListarTodos();
        Task<List<Voluntario>> ListarPorEvento(int eventoId);
    }
}