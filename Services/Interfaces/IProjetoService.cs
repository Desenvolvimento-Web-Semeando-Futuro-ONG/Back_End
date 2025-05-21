using System.Collections.Generic;
using System.Threading.Tasks;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Services.Interfaces
{
    public interface IProjetoService
    {
        // Métodos públicos
        Task<List<Projeto>> ListarProjetosAtivos();
        Task<List<Projeto>> ListarProjetosPorTipo(string tipo);
        Task<Projeto?> ObterProjetoPorId(int id);

        // Métodos para ADMs
        Task<List<Projeto>> ListarTodosProjetosAdmin(int admId);
        Task<Projeto> CriarProjetoAdmin(ProjetoViewModel model, int admId);
        Task<Projeto?> AtualizarProjetoAdmin(int id, ProjetoViewModel model, int admId);
        Task<bool> DesativarProjetoAdmin(int id, int admId);
        Task<Projeto?> AtivarProjeto(int id, int admId);
        Task<List<Projeto>> ListarProjetosDesativados(int admId);

        // Métodos para voluntários
        Task<bool> InscreverVoluntario(int projetoId, int voluntarioId);
        Task<bool> CancelarInscricao(int projetoId, int voluntarioId);
        Task<IActionResult> CadastrarVoluntarioComInscricao([FromBody] CadastroComInscricaoViewModel model);

        // Métodos para ADMs gerenciarem voluntários
        Task<bool> AprovarVoluntario(int projetoId, int voluntarioId, int admId);
        Task<bool> RejeitarVoluntario(int projetoId, int voluntarioId, int admId);
        Task<List<Voluntario>> ListarVoluntariosPorProjeto(int projetoId, int admId);
    }
}