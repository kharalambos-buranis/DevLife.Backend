using DevLife_Portal.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevLife_Portal.Common.Models
{
    public class CodeSnippet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Slug { get; set; }
        public string Language { get; set; }       
        public Experience Experience { get; set; }   
        public string CorrectCode { get; set; }
        public string BuggyCode { get; set; }
        public string Explanation { get; set; }
    }
}
