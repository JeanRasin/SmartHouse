using MongoDB.Bson.Serialization.Attributes;

namespace SmartHouse.Domain.Core
{
    public class EventId
    {
        [BsonElement("stateId")]
        public int StateId { get; set; }
        [BsonElement("Name")]
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
