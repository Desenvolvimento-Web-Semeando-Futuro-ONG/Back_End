using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Back_End.Services.Interfaces;
using Back_End.Enums;

namespace Back_End.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly AppDbContext _context;

        public ProjetoService(AppDbContext context)
        {
            _context = context;
        }

        // Método auxiliar para verificar e obter ID de administrador
        private async Task<(bool success, int id)> GetAdminId(int usuarioId)
        {
            var admin = await _context.Adms
                .FirstOrDefaultAsync(a => a.Id == usuarioId);

            return (admin != null, admin?.Id ?? 0);
        }

        // Método auxiliar para verificar e obter ID de voluntário
        private async Task<(bool success, int id)> GetVoluntarioId(int usuarioId)
        {
            var voluntario = await _context.Voluntarios
                .FirstOrDefaultAsync(v => v.Id == usuarioId);

            return (voluntario != null, voluntario?.Id ?? 0);
        }

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

        public async Task<List<Projeto>> ListarTodosProjetosAdmin(int usuarioId)
        {
            var (isAdmin, adminId) = await GetAdminId(usuarioId);
            if (!isAdmin) return new List<Projeto>();

            return await _context.Projetos
                .Where(p => p.CriadoPorAdmId == adminId)
                .Include(p => p.Voluntarios)
                .ThenInclude(pv => pv.Voluntario)
                .ToListAsync();
        }

        public async Task<Projeto> CriarProjetoAdmin(ProjetoViewModel model, int usuarioId)
        {
            var (isAdmin, adminId) = await GetAdminId(usuarioId);
            if (!isAdmin) throw new InvalidOperationException("Somente administradores podem criar projetos");

            var projeto = new Projeto
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                DataInicio = model.DataInicio,
                DataFim = model.DataFim,
                EhEventoEspecifico = model.EhEventoEspecifico,
                TipoEventoEspecifico = model.TipoEventoEspecifico,
                Status = StatusProjeto.Ativo,
                CriadoPorAdmId = adminId
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
            return projeto;
        }

        public async Task<Projeto?> AtualizarProjetoAdmin(int id, ProjetoViewModel model, int usuarioId)
        {
            var (isAdmin, adminId) = await GetAdminId(usuarioId);
            if (!isAdmin) return null;

            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == id && p.CriadoPorAdmId == adminId);

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

        public async Task<bool> DesativarProjetoAdmin(int id, int usuarioId)
        {
            var (isAdmin, adminId) = await GetAdminId(usuarioId);
            if (!isAdmin) return false;

            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == id && p.CriadoPorAdmId == adminId);

            if (projeto == null) return false;

            projeto.Status = StatusProjeto.Inativo;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InscreverVoluntario(int projetoId, int usuarioId)
        {
            var (isVoluntario, voluntarioId) = await GetVoluntarioId(usuarioId);
            if (!isVoluntario) return false;

            var jaInscrito = await _context.ProjetoVoluntarios
                .AnyAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (jaInscrito) return false;

            var inscricao = new ProjetoVoluntario
            {
                ProjetoId = projetoId,
                VoluntarioId = voluntarioId,
                Status = StatusInscricao.Pendente,
                DataInscricao = DateTime.UtcNow
            };

            _context.ProjetoVoluntarios.Add(inscricao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelarInscricao(int projetoId, int usuarioId)
        {
            var (isVoluntario, voluntarioId) = await GetVoluntarioId(usuarioId);
            if (!isVoluntario) return false;

            var inscricao = await _context.ProjetoVoluntarios
                .FirstOrDefaultAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (inscricao == null) return false;

            _context.ProjetoVoluntarios.Remove(inscricao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IActionResult> CadastrarVoluntarioComInscricao(CadastroComInscricaoViewModel model)
        {
            if (model == null)
            {
                return new BadRequestObjectResult(new
                {
                    sucesso = false,
                    mensagem = "Dados inválidos"
                });
            }

            if (!model.ProjetoId.HasValue || model.ProjetoId.Value <= 0)
            {
                return new BadRequestObjectResult(new
                {
                    sucesso = false,
                    mensagem = "ID do projeto inválido ou não fornecido"
                });
            }

            int projetoId = model.ProjetoId.Value;

            try
            {
                var cpfDigits = model.CPF?.Where(char.IsDigit).ToArray();
                var cpf = cpfDigits != null ? new string(cpfDigits) : string.Empty;

                if (string.IsNullOrWhiteSpace(cpf) || cpf.Length < 11)
                {
                    return new BadRequestObjectResult(new
                    {
                        sucesso = false,
                        mensagem = "CPF inválido"
                    });
                }

                var voluntario = await ProcessarVoluntario(model, cpf);
                if (voluntario == null)
                {
                    return new ConflictObjectResult(new
                    {
                        sucesso = false,
                        mensagem = "Falha ao processar dados do voluntário"
                    });
                }

                if (!await _context.Projetos.AnyAsync(p => p.Id == projetoId))
                {
                    return new NotFoundObjectResult(new
                    {
                        sucesso = false,
                        mensagem = "Projeto não encontrado"
                    });
                }

                var funcao = string.IsNullOrWhiteSpace(model.FuncaoDesejada) ? "Não especificada" : model.FuncaoDesejada;
                var resultadoInscricao = await ProcessarInscricao(projetoId, voluntario.Id, funcao);

                if (!resultadoInscricao.sucesso)
                {
                    return new ConflictObjectResult(new
                    {
                        sucesso = false,
                        mensagem = resultadoInscricao.mensagem
                    });
                }

                return new OkObjectResult(new
                {
                    sucesso = true,
                    mensagem = "Operação realizada com sucesso",
                    voluntarioId = voluntario.Id,
                    projetoId = projetoId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                return new ObjectResult(new
                {
                    sucesso = false,
                    mensagem = "Erro interno no servidor"
                })
                {
                    StatusCode = 500
                };
            }
        }

        private async Task<Voluntario> ProcessarVoluntario(CadastroComInscricaoViewModel model, string cpf)
        {
            var voluntario = await _context.Voluntarios
                .FirstOrDefaultAsync(v => v.CPF == cpf);

            if (voluntario == null)
            {
                voluntario = new Voluntario
                {
                    Nome = model.Nome ?? string.Empty,
                    CPF = cpf,
                    Email = model.Email ?? string.Empty,
                    Telefone = model.Telefone ?? string.Empty,
                    Habilidades = model.Habilidades ?? string.Empty,
                    Disponibilidade = model.Disponibilidade ?? string.Empty,
                    DataCadastro = DateTime.UtcNow
                };
                _context.Voluntarios.Add(voluntario);
            }
            else
            {
                voluntario.Nome = model.Nome ?? voluntario.Nome;
                voluntario.Email = model.Email ?? voluntario.Email;
                voluntario.Telefone = model.Telefone ?? voluntario.Telefone;
                voluntario.Habilidades = model.Habilidades ?? voluntario.Habilidades;
                voluntario.Disponibilidade = model.Disponibilidade ?? voluntario.Disponibilidade;
            }

            await _context.SaveChangesAsync();
            return voluntario;
        }

        private async Task<(bool sucesso, string mensagem)> ProcessarInscricao(int projetoId, int voluntarioId, string funcaoDesejada)
        {
            if (await _context.ProjetoVoluntarios
                .AnyAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId))
            {
                return (false, "Você já está inscrito neste projeto");
            }

            var inscricao = new ProjetoVoluntario
            {
                ProjetoId = projetoId,
                VoluntarioId = voluntarioId,
                Status = StatusInscricao.Pendente,
                FuncaoDesejada = funcaoDesejada,
                DataInscricao = DateTime.UtcNow
            };

            _context.ProjetoVoluntarios.Add(inscricao);
            await _context.SaveChangesAsync();

            return (true, string.Empty);
        }

        public async Task<bool> AprovarVoluntario(int projetoId, int voluntarioId, int admId)
        {
            var (isAdmin, _) = await GetAdminId(admId);
            if (!isAdmin) return false;

            var (isVoluntario, _) = await GetVoluntarioId(voluntarioId);
            if (!isVoluntario) return false;

            var inscricao = await _context.ProjetoVoluntarios
                .FirstOrDefaultAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (inscricao == null) return false;

            inscricao.Status = StatusInscricao.Aprovado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejeitarVoluntario(int projetoId, int voluntarioId, int admId)
        {
            var (isAdmin, _) = await GetAdminId(admId);
            if (!isAdmin) return false;

            var (isVoluntario, _) = await GetVoluntarioId(voluntarioId);
            if (!isVoluntario) return false;

            var inscricao = await _context.ProjetoVoluntarios
                .FirstOrDefaultAsync(pv => pv.ProjetoId == projetoId && pv.VoluntarioId == voluntarioId);

            if (inscricao == null) return false;

            _context.ProjetoVoluntarios.Remove(inscricao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Voluntario>> ListarVoluntariosPorProjeto(int projetoId, int admId)
        {
            var (isAdmin, _) = await GetAdminId(admId);
            if (!isAdmin) return new List<Voluntario>();

            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.Id == projetoId && p.CriadoPorAdmId == admId);

            if (projeto == null) return new List<Voluntario>();

            var voluntariosIds = await _context.ProjetoVoluntarios
                .Where(pv => pv.ProjetoId == projetoId)
                .Select(pv => pv.VoluntarioId)
                .ToListAsync();

            return await _context.Voluntarios
                .Where(v => voluntariosIds.Contains(v.Id))
                .ToListAsync();
        }
    }
}