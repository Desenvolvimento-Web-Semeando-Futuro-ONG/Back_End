using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.EntityFrameworkCore;
using Back_End.Services.Interfaces;


namespace Back_End.Services
{
    public class VoluntarioService : IVoluntarioService
    {
        private readonly AppDbContext _context;

        public VoluntarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Voluntario?> ObterPorCpf(string cpf)
        {
            return await _context.Voluntarios
                .FirstOrDefaultAsync(v => v.CPF == cpf);
        }

        public async Task<Voluntario> Cadastrar(VoluntarioViewModel model)
        {
            var voluntario = new Voluntario
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
            return voluntario;
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