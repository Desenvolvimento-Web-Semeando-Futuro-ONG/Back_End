using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Adm> Adms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabelas personalizadas (TPT)
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios");

            modelBuilder.Entity<Adm>()
                .ToTable("Adms"); // Herda de Usuario, mas com TPT (tabela separada)

            base.OnModelCreating(modelBuilder);
        }
    }
}
