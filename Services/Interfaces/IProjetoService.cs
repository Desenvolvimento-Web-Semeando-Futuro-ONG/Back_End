using System.Collections.Generic;
using System.Threading.Tasks;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Back_End.Services.Interfaces
{
    public interface IProjetoService
    {
        Task<List<Projeto>> ListarProjetosAtivos();
        Task<List<Projeto>> ListarProjetosPorTipo(string tipo);
        Task<Projeto?> ObterProjetoPorId(int id);

        Task<List<Projeto>> ListarTodosProjetosAdmin(int admId);
        Task<Projeto> CriarProjetoAdmin(ProjetoViewModel model, int admId);
        Task<Projeto?> AtualizarProjetoAdmin(int id, ProjetoViewModel model, int admId);
        Task<Projeto?> AtivarProjeto(int id, int admId);
        Task<Projeto?> DesativarProjeto(int id, int admId);
        Task<List<Projeto>> ListarProjetosDesativados(int admId);

        Task<bool> InscreverVoluntario(int projetoId, int voluntarioId);
        Task<bool> CancelarInscricao(int projetoId, int voluntarioId);
        Task<IActionResult> CadastrarVoluntarioComInscricao([FromBody] CadastroComInscricaoViewModel model);

        Task<bool> AprovarVoluntario(int projetoId, int voluntarioId, int admId);
        Task<bool> RejeitarVoluntario(int projetoId, int voluntarioId, int admId);
        Task<List<Voluntario>> ListarVoluntariosPorProjeto(int projetoId, int admId);
    }
}