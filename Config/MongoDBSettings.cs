namespace Back_End.Config
{
    public class MongoDBSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string GaleriaCollectionName { get; set; }
    }
}