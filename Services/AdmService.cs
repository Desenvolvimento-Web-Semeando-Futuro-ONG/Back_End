using System;
using Back_End.Data;
using Back_End.Models;
using Back_End.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Back_End.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Back_End.Enums;
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Configuration;

namespace Back_End.Services
{
    public class AdmService : IAdmService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly GaleriaService _galeriaService;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public AdmService(AppDbContext context,
                        IConfiguration configuration,
                        GaleriaService galeriaService,
                        IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _configuration = configuration;
            _galeriaService = galeriaService;
            _httpContextAccessor = httpContextAccessor; 
        }

        public async Task<string> Login(LoginViewModel loginVM)
        {
            if (loginVM == null || string.IsNullOrEmpty(loginVM.Login))
            {
                throw new ArgumentException("Credenciais inválidas");
            }

            var adm = await _context.Adms.FirstOrDefaultAsync(a => a.Login == loginVM.Login);
            if (adm == null || !adm.Autenticar(loginVM.Senha))
                throw new UnauthorizedAccessException("Login ou senha inválidos");

            return GerarTokenJwt(adm);
        }

        public async Task<int> CriarEvento(EventoViewModel model, int admId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var evento = new Evento
            {
                Nome = model.Nome ?? throw new ArgumentException("Nome do evento é obrigatório"),
                Descricao = model.Descricao ?? throw new ArgumentException("Descrição do evento é obrigatória"),
                DataEvento = model.DataEvento,
                CriadoPorAdmId = admId,
                ImagemUrl = model.Imagem != null ? await _galeriaService.UploadImagem(model.Imagem) : null
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento.Id;
        }

        public async Task<bool> EscalarVoluntarios(int eventoId, List<int> voluntariosIds)
        {
            var evento = await _context.Eventos
                .Include(e => e.Voluntarios)
                .FirstOrDefaultAsync(e => e.Id == eventoId);

            if (evento == null) return false;

            // Remove voluntários não selecionados
            var voluntariosParaRemover = evento.Voluntarios
                .Where(v => !voluntariosIds.Contains(v.VoluntarioId))
                .ToList();

            foreach (var vol in voluntariosParaRemover)
            {
                evento.Voluntarios.Remove(vol);
            }

            // Adiciona novos voluntários
            foreach (var volId in voluntariosIds)
            {
                if (!evento.Voluntarios.Any(v => v.VoluntarioId == volId))
                {
                    evento.Voluntarios.Add(new EventoVoluntario { VoluntarioId = volId });
                }
            }

            await _context.SaveChangesAsync();
            return true;
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

        public async Task<string> AdicionarFotoGaleria(IFormFile foto)
        {
            return await _galeriaService.UploadImagem(foto);
        }

        public async Task<bool> RemoverFotoGaleria(string fotoId)
        {
            return await _galeriaService.DeletarImagem(fotoId);
        }

        public async Task<string> PublicarTexto(string texto)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("id");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int admId))
            {
                throw new UnauthorizedAccessException("ID do administrador não encontrado ou inválido");
            }

            var publicacao = new Publicacao
            {
                Texto = texto,
                DataPublicacao = DateTime.UtcNow,
                AdmId = admId
            };

            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();
            return "Texto publicado com sucesso";
        }

        public async Task<int> CriarAdm(AdmViewModel admVM)
        {
            try
            {
                if (await _context.Adms.AnyAsync(a => a.Login == admVM.Login))
                    throw new ArgumentException("Login já está em uso");

                if (await _context.Adms.AnyAsync(a => a.CPF == admVM.CPF))
                    throw new ArgumentException("CPF já cadastrado");

                if (await _context.Adms.AnyAsync(a => a.Email == admVM.Email))
                    throw new ArgumentException("Email já cadastrado");

                var adm = new Adm
                {
                    Nome = admVM.Nome,
                    Telefone = admVM.Telefone,
                    CPF = admVM.CPF,
                    Email = admVM.Email,
                    Login = admVM.Login,
                };

                adm.DefinirSenha(admVM.Senha);

                _context.Adms.Add(adm);
                await _context.SaveChangesAsync();

                return adm.Id;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao criar administrador: {ex.InnerException?.Message}");
                throw new Exception("Erro ao salvar no banco de dados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar administrador: {ex.Message}");
                throw;
            }
        }

        public async Task<List<AdmRespostaViewModel>> ListarAdms()
        {
            return await _context.Adms
                .Select(a => new AdmRespostaViewModel
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Login = a.Login
                })
                .ToListAsync();
        }

        public async Task<bool> EditarAdm(int id, EditarAdmViewModel admVM)
        {
            var adm = await _context.Adms.FindAsync(id);
            if (adm == null) return false;

            if (!string.IsNullOrEmpty(admVM.Nome))
                adm.Nome = admVM.Nome;

            if (!string.IsNullOrEmpty(admVM.Login))
            {
                if (await _context.Adms.AnyAsync(a => a.Login == admVM.Login && a.Id != id))
                    throw new ArgumentException("Login já está em uso");

                adm.Login = admVM.Login;
            }

            if (!string.IsNullOrEmpty(admVM.Senha))
                adm.DefinirSenha(admVM.Senha);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AdmRespostaViewModel?> ObterPerfilAdm(int id) 
        {
            var adm = await _context.Adms.FindAsync(id);
            if (adm == null) return null;

            return new AdmRespostaViewModel
            {
                Id = adm.Id,
                Nome = adm.Nome,
                Telefone = adm.Telefone,
                CPF = adm.CPF,
                Email = adm.Email,
                Login = adm.Login
            };
        }

        private string GerarTokenJwt(Adm adm)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, adm.Nome),
            new Claim(ClaimTypes.Role, "Adm"),
            new Claim("id", adm.Id.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}