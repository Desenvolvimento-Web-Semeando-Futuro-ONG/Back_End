using Back_End.ViewModels;
using Back_End.Models;

namespace Back_End.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<string?> LoginAdm(LoginViewModel loginVM);
        string GerarTokenVoluntario(Voluntario voluntario);
        string GerarTokenAdm(Adm adm);
        Task<bool> SolicitarRedefinicaoSenha(string email);
        Task<bool> RedefinirSenha(RedefinirSenhaViewModel redefinirSenhaVM);
    }
}