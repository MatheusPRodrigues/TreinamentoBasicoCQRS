using CQRS.Infraestructure.Context.MongoConfig;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CQRS.Infraestructure.Context
{
    public class ReadContext
    {        
        private readonly IMongoDatabase _database;

        public ReadContext(IOptions<MongoDBSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionURI);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}
