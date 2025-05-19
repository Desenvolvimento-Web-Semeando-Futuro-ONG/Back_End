using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Back_End.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Services
{
    public class VoluntarioService : IVoluntarioService
    {
        private readonly AppDbContext _context;
        private readonly IAutenticacaoService _autenticacaoService;

        public VoluntarioService(AppDbContext context, IAutenticacaoService autenticacaoService)
        {
            _context = context;
            _autenticacaoService = autenticacaoService;
        }

        public async Task<RespostaCadastroVoluntario> Cadastrar(VoluntarioViewModel model)
        {
            var voluntarioExistente = await _context.Voluntarios
                .FirstOrDefaultAsync(v => v.CPF == model.CPF);

            Voluntario voluntario;

            if (voluntarioExistente != null)
            {
                voluntarioExistente.Nome = model.Nome;
                voluntarioExistente.Telefone = model.Telefone;
                voluntarioExistente.Email = model.Email;
                voluntarioExistente.Habilidades = model.Habilidades;
                voluntarioExistente.Disponibilidade = model.Disponibilidade;

                voluntario = voluntarioExistente;
                await _context.SaveChangesAsync();
            }
            else
            {
                voluntario = new Voluntario
                {
                    Nome = model.Nome,
                    Telefone = model.Telefone,
                    CPF = model.CPF,
                    Email = model.Email,
                    Habilidades = model.Habilidades,
                    Disponibilidade = model.Disponibilidade
                };

                _context.Voluntarios.Add(voluntario);
                await _context.SaveChangesAsync();
            }

            return new RespostaCadastroVoluntario
            {
                Voluntario = voluntario,
                Token = _autenticacaoService.GerarTokenVoluntario(voluntario),
                Mensagem = voluntarioExistente != null
                    ? "Dados atualizados com sucesso"
                    : "Cadastro realizado com sucesso"
            };
        }

        public async Task<Voluntario?> ObterPorCpf(string cpf)
        {
            return await _context.Voluntarios
                .FirstOrDefaultAsync(v => v.CPF == cpf);
        }

        public async Task<List<Voluntario>> ListarTodos()
        {
            return await _context.Voluntarios.ToListAsync();
        }

        public async Task<List<Voluntario>> ListarPorEvento(int eventoId)
        {
            return await _context.EventoVoluntarios
                .Where(ev => ev.EventoId == eventoId)
                .Select(ev => ev.Voluntario!)
                .ToListAsync();
        }
    }
}
