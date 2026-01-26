using CQRS.Infraestructure.Context.MongoConfig;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CQRS.Infraestructure.Context
{
    public class MongoContext : IAbstractFactory<IMongoDatabase>
    {        
        private readonly IMongoDatabase _database;

        public MongoContext(IOptions<MongoDBSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionURI);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoDatabase CreateConnection()
        {
            return _database;
        }
    }
}
