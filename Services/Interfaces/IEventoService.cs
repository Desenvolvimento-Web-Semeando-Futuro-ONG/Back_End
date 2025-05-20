using Back_End.Models;
using Back_End.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Back_End.Services.Interfaces
{
    public interface IEventoService
    {
        Task<List<Evento>> ListarTodos();
        Task<List<Evento>> ListarRascunhos(int admId);
        Task<List<Evento>> ListarPublicados();
        Task<Evento?> ObterPorId(int id);
        Task<Evento> Criar(EventoViewModel model, int admId, GaleriaService galeriaService);
        Task<Evento?> Atualizar(int id, EventoViewModel model, GaleriaService galeriaService);
        Task<bool> PublicarRascunho(int id);
        Task<bool> Deletar(int id);
    }
}