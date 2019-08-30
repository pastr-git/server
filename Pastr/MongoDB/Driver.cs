using MongoDB.Driver;

namespace Pastr.MongoDB
{
    public class Driver
    {
        public MongoClient Client { get; set; }

        public IMongoDatabase Database { get; set; }

        public string DatabaseName { get; } = "pastr";

        public Driver()
        {
            Client = new MongoClient();
        }

        public bool LoadDatabase()
        {
            Database = Client.GetDatabase(DatabaseName);
            return Database != null;
        }

        public IMongoCollection<T> GetCollection<T>() where T : IModel, new()
        {
            return Database.GetCollection<T>(new T().CollectionName);
        }
    }
}
