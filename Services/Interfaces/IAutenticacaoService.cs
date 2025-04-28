using Back_End.ViewModels;
using Back_End.Models;

namespace Back_End.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        //Task<string?> Login(LoginViewModel loginVM);
        string GerarTokenJwt(Adm adm);
    }
}