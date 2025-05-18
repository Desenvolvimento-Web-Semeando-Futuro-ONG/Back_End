using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.EntityFrameworkCore;
using Back_End.Services.Interfaces;

namespace Back_End.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly AppDbContext _context;

        public ProjetoService(AppDbContext context)
        {
            _context = context;
        }

        // Métodos públicos
        public async Task<List<Projeto>> ListarProjetosAtivos()
        {
            return await _context.Projetos
                .Where(p => p.Status == StatusProjeto.Ativo)
                .Include(p => p.Voluntarios.Where(pv => pv.Status == StatusInscricao.Aprovado))
                .ThenInclude(pv => pv.Voluntario)
                .ToListAsync();
        }

        public async Task<List<Projeto>> ListarProjetosPorTipo(string tipo)
        {
            return await _context.Projetos
                .Where(p => p.Status == StatusProjeto.Ativo &&
                           p.EhEventoEspecifico &&
                           p.TipoEventoEspecifico == tipo)
                .ToListAsync();
        }

        public async Task<Projeto?> ObterProjetoPorId(int id)
        {
            return await _context.Projetos
                .Include(p => p.Voluntarios)
                .ThenInclude(pv => pv.Voluntario)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Métodos para ADMs
        public async Task<List<Projeto>> ListarTodosProjetosAdmin(int admId)
        {
            return await _context.Projetos
                .Where(p => p.CriadoPorAdmId == admId)
                .Include(p => p.Voluntarios)
                .ThenInclude(pv => pv.Voluntario)
                .ToListAsync();
        }

        public async Task<Projeto> CriarProjetoAdmin(ProjetoViewModel model, int admId)
        {
            var projeto = new Projeto
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                DataInicio = model.DataInicio,
                DataFim = model.DataFim,
                EhEventoEspecifico = model.EhEventoEspecifico,
                TipoEventoEspecifico = model.TipoEventoEspecifico,
                Status = StatusProjeto.Ativo,
                CriadoPorAdmId = admId
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
            return projeto;
        }

        public async Task<Projeto?> AtualizarProjetoAdmin(int id, ProjetoViewModel model, int admId)
        {
            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == id && p.CriadoPorAdmId == admId);

            if (projeto == null) return null;

            projeto.Nome = model.Nome;
            projeto.Descricao = model.Descricao;
            projeto.DataInicio = model.DataInicio;
            projeto.DataFim = model.DataFim;
            projeto.EhEventoEspecifico = model.EhEventoEspecifico;
            projeto.TipoEventoEspecifico = model.TipoEventoEspecifico;

            await _context.SaveChangesAsync();
            return projeto;
        }

        public async Task<bool> DesativarProjetoAdmin(int id, int admId)
        {
            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == id && p.CriadoPorAdmId == admId);

            if (projeto == null) return false;

            projeto.Status = StatusProjeto.Inativo;
            await _context.SaveChangesAsync();
            return true;
        }

        // Métodos para voluntários
        public async Task<bool> InscreverVoluntario(int projetoId, int voluntarioId)
        {
            var existe = await _context.ProjetoVoluntarios
                .AnyAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (existe) return false;

            var inscricao = new ProjetoVoluntario
            {
                ProjetoId = projetoId,
                VoluntarioId = voluntarioId,
                Status = StatusInscricao.Pendente,
                DataInscricao = DateTime.Now
            };

            _context.ProjetoVoluntarios.Add(inscricao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelarInscricao(int projetoId, int voluntarioId)
        {
            var inscricao = await _context.ProjetoVoluntarios
                .FirstOrDefaultAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (inscricao == null) return false;

            _context.ProjetoVoluntarios.Remove(inscricao);
            await _context.SaveChangesAsync();
            return true;
        }

        // Métodos para ADMs gerenciarem voluntários
        public async Task<bool> AprovarVoluntario(int projetoId, int voluntarioId, int admId)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto?.CriadoPorAdmId != admId) return false;

            var inscricao = await _context.ProjetoVoluntarios
                .FirstOrDefaultAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (inscricao == null) return false;

            inscricao.Status = StatusInscricao.Aprovado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejeitarVoluntario(int projetoId, int voluntarioId, int admId)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto?.CriadoPorAdmId != admId) return false;

            var inscricao = await _context.ProjetoVoluntarios
                .FirstOrDefaultAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (inscricao == null) return false;

            _context.ProjetoVoluntarios.Remove(inscricao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Voluntario>> ListarVoluntariosPorProjeto(int projetoId, int admId)
        {
            var projeto = await _context.Projetos.FindAsync(projetoId);
            if (projeto?.CriadoPorAdmId != admId) return new List<Voluntario>();

            return await _context.ProjetoVoluntarios
                .Where(pv => pv.ProjetoId == projetoId)
                .Include(pv => pv.Voluntario)
                .Select(pv => pv.Voluntario!)
                .ToListAsync();
        }
    }
}