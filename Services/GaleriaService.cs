using Back_End.Config;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace Back_End.Services
{
    public class GaleriaService
    {
        private readonly IMongoCollection<ImagemModel> _imagensCollection;

        public GaleriaService(IOptions<MongoDBSettings> settings)
        {
            var mongoSettings = settings.Value;
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _imagensCollection = database.GetCollection<ImagemModel>(mongoSettings.CollectionName);
        }

        public async Task<string> UploadImagem(IFormFile imagem)
        {
            using var memoryStream = new MemoryStream();
            await imagem.CopyToAsync(memoryStream);

            var imagemModel = new ImagemModel
            {
                Id = ObjectId.GenerateNewId(),
                NomeArquivo = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}",
                Dados = memoryStream.ToArray(),
                ContentType = imagem.ContentType,
                DataUpload = DateTime.UtcNow
            };

            await _imagensCollection.InsertOneAsync(imagemModel);
            return imagemModel.Id.ToString();
        }

        public async Task<ImagemModel?> ObterImagem(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            return await _imagensCollection.Find(i => i.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task<bool> DeletarImagem(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var result = await _imagensCollection.DeleteOneAsync(i => i.Id == objectId);
            return result.DeletedCount > 0;
        }
    }

    public class ImagemModel
    {
        public ObjectId Id { get; set; }
        public string NomeArquivo { get; set; } = null!;
        public byte[] Dados { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public DateTime DataUpload { get; set; }
    }
}