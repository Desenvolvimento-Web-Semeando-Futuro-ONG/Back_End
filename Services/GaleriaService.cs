using Back_End.Config;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Back_End.Services
{
    public class GaleriaService
    {
        private readonly IMongoCollection<ImagemModel> _imagensCollection;

        public GaleriaService(MongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _imagensCollection = database.GetCollection<ImagemModel>("Imagens");
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
            return await _imagensCollection.Find(i => i.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<bool> DeletarImagem(string id)
        {
            var result = await _imagensCollection.DeleteOneAsync(i => i.Id == ObjectId.Parse(id));
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