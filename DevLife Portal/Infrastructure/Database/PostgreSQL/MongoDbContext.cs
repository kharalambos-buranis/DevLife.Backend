using DevLife_Portal.Common.Models;
using MongoDB.Driver;

namespace DevLife_Portal.Infrastructure.Database.PostgreSQL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            _db = client.GetDatabase(config["MongoDb:Database"]);
        }

        public IMongoCollection<CodeSnippet> CodeSnippets => _db.GetCollection<CodeSnippet>("casino_snippets");
    }
}
