using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.EntityFrameworkCore;
using Back_End.Services.Interfaces;


namespace Back_End.Services
{
    public class DoadorService : IDoadorService
    {
        private readonly AppDbContext _context;

        public DoadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Doador?> ObterPorCpf(string cpf)
        {
            return await _context.Doadores
                .FirstOrDefaultAsync(d => d.CPF == cpf);
        }

        public async Task<Doador> Cadastrar(DoadorViewModel model)
        {
            var doador = new Doador
            {
                Nome = model.Nome,
                Telefone = model.Telefone,
                CPF = model.CPF,
                Email = model.Email
            };

            _context.Doadores.Add(doador);
            await _context.SaveChangesAsync();
            return doador;
        }

        public async Task<List<Doador>> ListarTodos()
        {
            return await _context.Doadores.ToListAsync();
        }

        public async Task<Doacao> RegistrarDoacao(int doadorId, decimal valor, string metodoPagamento)
        {
            var doacao = new Doacao
            {
                DoadorId = doadorId,
                Valor = valor,
                DataDoacao = DateTime.UtcNow,
                MetodoPagamento = metodoPagamento
            };

            _context.Doacoes.Add(doacao);
            await _context.SaveChangesAsync();
            return doacao;
        }
    }
}