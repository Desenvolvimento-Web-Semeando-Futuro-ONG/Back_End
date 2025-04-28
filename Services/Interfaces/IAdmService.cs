using Back_End.ViewModels;

namespace Back_End.Services.Interfaces
{
    public interface IAdmService
    {
        Task<string> Login(LoginViewModel loginVM);
        Task<int> CriarEvento(EventoViewModel model, int admId);
        Task<bool> EscalarVoluntarios(int eventoId, List<int> voluntariosIds);
        Task<List<DoacaoViewModel>> ListarDoacoes();
        Task<string> AdicionarFotoGaleria(IFormFile foto);
        Task<bool> RemoverFotoGaleria(string fotoId);
        Task<string> PublicarTexto(string texto);
    }
}