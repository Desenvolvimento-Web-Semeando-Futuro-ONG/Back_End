using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Back_End.Services
{
    public class EventoService : IEventoService
    {
        private readonly AppDbContext _context;
        private readonly GaleriaService _galeriaService;

        public EventoService(AppDbContext context, GaleriaService galeriaService)
        {
            _context = context;
            _galeriaService = galeriaService;
        }

        public async Task<List<Evento>> ListarTodos()
        {
            return await _context.Eventos
                .Include(e => e.Voluntarios)
                .ThenInclude(ev => ev.Voluntario)
                .Include(e => e.CriadoPorAdm)
                .ToListAsync();
        }

        public async Task<List<Evento>> ListarRascunhos(int admId)
        {
            return await _context.Eventos
                .Where(e => e.EhRascunho && e.CriadoPorAdmId == admId)
                .Include(e => e.Voluntarios)
                .ThenInclude(ev => ev.Voluntario)
                .Include(e => e.CriadoPorAdm)
                .ToListAsync();
        }

        public async Task<List<Evento>> ListarPublicados()
        {
            return await _context.Eventos
                .Where(e => !e.EhRascunho)
                .Include(e => e.Voluntarios)
                .ThenInclude(ev => ev.Voluntario)
                .Include(e => e.CriadoPorAdm)
                .ToListAsync();
        }

        public async Task<Evento?> ObterPorId(int id)
        {
            return await _context.Eventos
                .Include(e => e.Voluntarios)
                .ThenInclude(ev => ev.Voluntario)
                .Include(e => e.CriadoPorAdm)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Evento> Criar(EventoViewModel model, int admId, GaleriaService galeriaService)
        {
            string? imagemUrl = null;

            if (model.Imagem != null)
            {
                imagemUrl = await galeriaService.UploadImagem(model.Imagem);
            }

            var evento = new Evento
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                DataEvento = model.DataEvento,
                ImagemUrl = imagemUrl,
                CriadoPorAdmId = admId,
                EhRascunho = model.SalvarComoRascunho,
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<Evento?> Atualizar(int id, EventoViewModel model, GaleriaService galeriaService)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return null;

            string? imagemUrl = null;
            if (model.Imagem != null)
            {
                if (!string.IsNullOrEmpty(evento.ImagemUrl))
                {
                    await galeriaService.DeletarImagem(evento.ImagemUrl);
                }
                imagemUrl = await galeriaService.UploadImagem(model.Imagem);
            }

            evento.Nome = model.Nome;
            evento.Descricao = model.Descricao;
            evento.DataEvento = model.DataEvento;
            if (imagemUrl != null) evento.ImagemUrl = imagemUrl;

            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<bool> PublicarRascunho(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null || !evento.EhRascunho) return false;

            evento.EhRascunho = false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return false;

            if (!string.IsNullOrEmpty(evento.ImagemUrl))
            {
                await _galeriaService.DeletarImagem(evento.ImagemUrl);
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}