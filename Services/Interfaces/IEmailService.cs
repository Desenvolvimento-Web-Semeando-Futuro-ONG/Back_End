using System.Threading.Tasks;

namespace Back_End.Services.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string email, string assunto, string mensagem);
    }
}