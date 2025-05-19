using Back_End.Models;
using Back_End.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Back_End.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Adm> Adms { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Doador> Doadores { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<EventoVoluntario> EventoVoluntarios { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<IntegracaoWhatsApp> IntegracoesWhatsApp { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<ProjetoVoluntario> ProjetoVoluntarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relação Projeto -> CriadoPorAdm
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.CriadoPorAdm)
                .WithMany()
                .HasForeignKey(p => p.CriadoPorAdmId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação muitos-para-muitos: Projeto <-> Voluntario
            modelBuilder.Entity<ProjetoVoluntario>()
                .HasKey(pv => new { pv.ProjetoId, pv.VoluntarioId });

            modelBuilder.Entity<ProjetoVoluntario>()
                .HasOne(pv => pv.Projeto)
                .WithMany(p => p.Voluntarios)
                .HasForeignKey(pv => pv.ProjetoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjetoVoluntario>()
                .HasOne(pv => pv.Voluntario)
                .WithMany(v => v.Projetos)
                .HasForeignKey(pv => pv.VoluntarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relação muitos-para-muitos: Evento <-> Voluntario
            modelBuilder.Entity<EventoVoluntario>()
                .HasKey(ev => new { ev.EventoId, ev.VoluntarioId });

            modelBuilder.Entity<EventoVoluntario>()
                .HasOne(ev => ev.Evento)
                .WithMany(e => e.Voluntarios)
                .HasForeignKey(ev => ev.EventoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventoVoluntario>()
                .HasOne(ev => ev.Voluntario)
                .WithMany(v => v.Eventos)
                .HasForeignKey(ev => ev.VoluntarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Definir comprimento máximo padrão para propriedades string
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null)
                {
                    property.SetMaxLength(256);
                }
            }
        }
    }
}
