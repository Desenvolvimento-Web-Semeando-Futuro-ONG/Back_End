using Back_End.Config;
using Back_End.Data;
using Back_End.Enums;
using Back_End.Models;
using Back_End.Services;
using Back_End.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Banco de Dados - PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MongoDB Configuração
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
builder.Services.AddSingleton<GaleriaService>();

// Autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<IAdmService, AdmService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IVoluntarioService, VoluntarioService>();
builder.Services.AddScoped<IDoadorService, DoadorService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API ONG", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Ativando o Swagger apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ONG v1");
        c.RoutePrefix = string.Empty; // Vai abrir o Swagger diretamente na raiz (http://localhost:5000/)
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Migração e Criação de Admin (caso não exista)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Adms.Any())
    {
        var admin = new Adm
        {
            Nome = "Admin",
            Email = "admin@ong.com",
            Telefone = "11999999999",
            CPF = "12345678901",
            Tipo = TipoUsuario.Adm,
            Login = "admin",
        };
        admin.DefinirSenha("123456");
        db.Adms.Add(admin);
        db.SaveChanges();
    }
}

// Migração de hashes (executar uma vez)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Verificando hashes para migração...");

        var admParaMigrar = await db.Adms
            .Where(a => a.SenhaHash.Length == 44 && a.SenhaHash.EndsWith("="))
            .ToListAsync();

        if (admParaMigrar.Any())
        {
            logger.LogInformation($"Migrando {admParaMigrar.Count} usuários...");

            foreach (var adm in admParaMigrar)
            {
                // Use a senha padrão ou peça para redefinir
                adm.DefinirSenha("BackMDS123"); // Substitua pela senha padrão correta
            }

            await db.SaveChangesAsync();
            logger.LogInformation("Migração concluída com sucesso!");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro durante a migração de hashes");
    }
}

app.Run();
