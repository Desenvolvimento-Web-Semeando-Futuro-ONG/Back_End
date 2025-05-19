using Back_End.ViewModels;
using Back_End.Models;

namespace Back_End.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<string?> LoginAdm(LoginViewModel loginVM);
        string GerarTokenAdm(Adm adm);
        string GerarTokenVoluntario(Voluntario voluntario);
    }
}