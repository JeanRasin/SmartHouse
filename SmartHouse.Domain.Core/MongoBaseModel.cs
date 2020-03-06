using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Domain.Core
{
    public class MongoBaseModel
    {
        [BsonId]
        [Required]
        public string Id { get; set; }
        [BsonElement("categoryName")]
        [BsonRepresentation(BsonType.String)]
        public string CategoryName { get; set; }
    }
}
