using Back_End.Models;
using Back_End.ViewModels;

namespace Back_End.Services.Interfaces
{
    public interface IEventoService
    {
        Task<List<Evento>> ListarTodos();
        Task<Evento?> ObterPorId(int id);
        Task<Evento> Criar(EventoViewModel model, int admId, GaleriaService galeriaService);
        Task<Evento?> Atualizar(int id, EventoViewModel model, GaleriaService galeriaService);
        Task<bool> Deletar(int id);
    }
}