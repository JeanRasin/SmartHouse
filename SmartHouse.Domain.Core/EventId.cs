using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHouse.Domain.Core
{
    public class EventId
    {
        [BsonElement("stateId")]
        [BsonRepresentation(BsonType.Int32)]
        public int StateId { get; set; }
        [BsonElement("name")]
        [BsonRepresentation(BsonType.String)]
        [BsonIgnoreIfNull]
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
