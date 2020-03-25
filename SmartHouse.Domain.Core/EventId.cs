using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SmartHouse.Domain.Core
{
    public class EventId
    {
        [BsonElement("stateId")]
        [BsonRepresentation(BsonType.Int32)]
        [JsonProperty(Required = Required.Always)]
        public int StateId { get; set; }

        [BsonElement("name")]
        [BsonRepresentation(BsonType.String)]
        [BsonIgnoreIfNull]
        [JsonProperty(Required = Required.AllowNull)]
        public string Name { get; set; }

        public EventId()
        {
        }

        public EventId(Microsoft.Extensions.Logging.EventId eventId)
        {
            StateId = eventId.Id;
            Name = eventId.Name;
        }
    }
}