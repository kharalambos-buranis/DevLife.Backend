using DevLife_Portal.Common.Enums;
using DevLife_Portal.Common.Models;
using MongoDB.Driver;

namespace DevLife_Portal.Infrastructure.Mongo
{
    public class MongoSeeder
    {
        private readonly IMongoCollection<CodeSnippet> _collection;

        public MongoSeeder(IMongoDatabase db)
        {
            _collection = db.GetCollection<CodeSnippet>("casino_snippets");
        }

        public async Task SeedAsync()
        {
            var existingCount = await _collection.CountDocumentsAsync(FilterDefinition<CodeSnippet>.Empty);

            if (existingCount > 0)
                return; // Already seeded

            var snippets = new List<CodeSnippet>
        {
            new()
            {
                Slug = "addition-001",
                Language = ".NET",
                Experience = Experience.Junior,
                CorrectCode = "int sum = a + b;",
                BuggyCode = "int sum = a - b;",
                Explanation = "Correct code adds a and b, buggy one subtracts instead."
            },
            new()
            {
                Slug = "loop-002",
                Language = ".NET",
                Experience = Experience.Middle,
                CorrectCode = "for(int i=0; i<10; i++) Console.WriteLine(i);",
                BuggyCode = "for(int i=10; i<0; i--) Console.WriteLine(i);",
                Explanation = "Buggy code has wrong loop condition and decrements instead of incrementing."
            },
            new()
            {
                Slug = "null-check-003",
                Language = ".NET",
                Experience = Experience.Senior,
                CorrectCode = "if (obj != null) Console.WriteLine(obj.ToString());",
                BuggyCode = "Console.WriteLine(obj.ToString());",
                Explanation = "Buggy code throws NullReferenceException if obj is null."
            },
            new()
            {
               Slug = "sum-python-001",
               Language = "Python",
               Experience = Experience.Junior,
               CorrectCode = "result = a + b\nprint(result)",
               BuggyCode = "result = a - b\nprint(result)",
               Explanation = "Correct code adds two numbers. Buggy version subtracts them."
            },
            new()
            {
               Slug = "loop-python-002",
               Language = "Python",
               Experience = Experience.Middle,
               CorrectCode = "for i in range(5):\n    print(i)",
               BuggyCode = "for i in range(5)\n    print(i)",
               Explanation = "Buggy code is missing a colon ':' after the for loop declaration."
            },
            new()
            {
              Slug = "null-check-python-003",
              Language = "Python",
              Experience = Experience.Senior,
              CorrectCode = "if obj is not None:\n    print(obj)",
              BuggyCode = "print(obj)",
              Explanation = "Buggy code may throw a NameError if `obj` is None or undefined."
            }
        };

            await _collection.InsertManyAsync(snippets);
        }
    }
}
