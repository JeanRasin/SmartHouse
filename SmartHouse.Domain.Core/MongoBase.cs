using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Domain.Core
{
    public class MongoBase
    {
        [BsonId]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

        [BsonElement("categoryName")]
        [BsonRepresentation(BsonType.String)]
        public string CategoryName { get; set; }
    }
}