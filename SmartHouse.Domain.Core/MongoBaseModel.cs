using MongoDB.Bson.Serialization.Attributes;

namespace SmartHouse.Domain.Core
{
    public class MongoBaseModel
    {
        [BsonElement("_id")]
        public string Id { get; set; }
    }
}
