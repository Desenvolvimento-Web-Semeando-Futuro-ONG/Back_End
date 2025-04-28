using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor padrão necessário para migrações
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Adm> Adms { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Doador> Doadores { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<EventoVoluntario> EventoVoluntarios { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<IntegracaoWhatsApp> IntegracoesWhatsApp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventoVoluntario>()
                .HasKey(ev => new { ev.EventoId, ev.VoluntarioId });

            modelBuilder.Entity<EventoVoluntario>()
                .HasOne(ev => ev.Evento)
                .WithMany(e => e.Voluntarios)
                .HasForeignKey(ev => ev.EventoId);

            modelBuilder.Entity<EventoVoluntario>()
                .HasOne(ev => ev.Voluntario)
                .WithMany(v => v.Eventos)
                .HasForeignKey(ev => ev.VoluntarioId);

            // Configurações adicionais de modelos...

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configuração usada apenas para migrações
                optionsBuilder.UseNpgsql("Host=dpg-cvunjkvdiees73e86n4g-a.oregon-postgres.render.com;Port=5432;Database=postgres_semear;Username=postgres_semear_user;Password=JxMLP4NlOWLcmej8QUN51C3Kz36FCGjP;SSL Mode=Require;Trust Server Certificate=true;Pooling=true;");
            }
        }
    }
}