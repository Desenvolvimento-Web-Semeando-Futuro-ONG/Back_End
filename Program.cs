using Back_End.Data;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servi�os
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativa Swagger em todas as execu��es
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = string.Empty; // Swagger abre automaticamente
});

// Pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Migrations autom�ticas e cria��o do Adm padr�o
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var admExistente = db.Adms.FirstOrDefault(a => a.Login == "admsemer");

    if (admExistente != null)
    {
        if (!admExistente.Autenticar("123456"))
        {
            admExistente.DefinirSenha("123456"); // Garante que a senha esteja com hash atualizado
            db.SaveChanges();
        }
    }
    else
    {
        var adm = new Adm
        {
            Nome = "AdmSemer",
            Email = "admsemer@teste.com",
            Telefone = "(81) 91234-5678",
            CPF = "12345678901",
            Tipo = "Adm",
            Login = "admsemer"
        };

        adm.DefinirSenha("123456");

        db.Adms.Add(adm);
        db.SaveChanges();
    }
}

app.Run();
