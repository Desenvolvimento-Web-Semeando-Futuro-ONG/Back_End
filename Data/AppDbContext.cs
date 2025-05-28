using Back_End.Models;
using Back_End.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Back_End.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Usuario> Usuarios { get; set; }
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
        public DbSet<HistoricoAprovacao> HistoricosAprovacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Usuario>().ToTable("Usuarios"); // tabela base

            //modelBuilder.Entity<Adm>().ToTable("Adms");
            //modelBuilder.Entity<Voluntario>().ToTable("Voluntarios");
            //modelBuilder.Entity<Doador>().ToTable("Doadores");

            //modelBuilder.Entity<Adm>(entity =>
            //{
            //    entity.HasIndex(a => a.CPF).IsUnique();
            //    entity.HasIndex(a => a.Email).IsUnique();
            //    entity.HasIndex(a => a.Login).IsUnique();
            //    entity.HasIndex(a => a.s).IsUnique();

            //});

            //modelBuilder.Entity<Voluntario>(entity =>
            //{
            //    entity.HasIndex(v => v.CPF).IsUnique();
            //    entity.HasIndex(v => v.Email).IsUnique();
            //});

            //modelBuilder.Entity<Doador>(entity =>
            //{
            //    entity.HasIndex(d => d.CPF).IsUnique();
            //    entity.HasIndex(d => d.Email).IsUnique();
            //});

            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.CriadoPorAdm)
                .WithMany()
                .HasForeignKey(p => p.CriadoPorAdmId)
                .OnDelete(DeleteBehavior.Restrict);

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

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null)
                {
                    property.SetMaxLength(256);
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
