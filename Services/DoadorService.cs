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

        public async Task<List<DoacaoViewModel>> ListarDoacoes()
        {
            return await _context.Doacoes
                .Include(d => d.Doador)
                .Where(d => d.Doador != null)
                .Select(d => new DoacaoViewModel
                {
                    Id = d.Id,
                    DoadorNome = d.Doador!.Nome,
                    Valor = d.Valor,
                    DataDoacao = d.DataDoacao,
                    MetodoPagamento = d.MetodoPagamento
                })
                .ToListAsync();
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

            if (model.ValorDoacao > 0)
            {
                await RegistrarDoacao(doador.Id, model.ValorDoacao, model.MetodoPagamento);
            }

            return doador;
        }

        public async Task<Doador> CadastrarComDoacao(DoadorViewModel model)
        {
            var doador = new Doador
            {
                Nome = model.Nome,
                Telefone = model.Telefone,
                CPF = model.CPF,
                Email = model.Email
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Doadores.Add(doador);
                await _context.SaveChangesAsync();

                if (model.ValorDoacao > 0)
                {
                    var doacao = new Doacao
                    {
                        DoadorId = doador.Id,
                        Valor = model.ValorDoacao,
                        DataDoacao = DateTime.UtcNow,
                        MetodoPagamento = model.MetodoPagamento
                    };
                    _context.Doacoes.Add(doacao);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return doador;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Doador>> ListarTodos()
        {
            return await _context.Doadores.ToListAsync();
        }

        public async Task<Doacao> RegistrarDoacao(int doadorId, decimal valor, string metodoPagamento)
        {
            var doador = await _context.Doadores.FindAsync(doadorId);
            if (doador == null)
                throw new ArgumentException("Doador não encontrado");

            var doacao = new Doacao
            {
                DoadorId = doadorId,
                Valor = valor,
                DataDoacao = DateTime.UtcNow,
                MetodoPagamento = metodoPagamento,
                ComprovanteUrl = null 
            };

            _context.Doacoes.Add(doacao);
            await _context.SaveChangesAsync();

            return doacao;
        }
    }
}