using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("logger")]
    public class LoggerModel : MongoBaseModel
    {
        [BsonElement("logLevel")]
        [BsonRepresentation(BsonType.Int32)]
        public Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; }

        [BsonElement("eventId")]
        [Required]
        public EventId EventId { get; set; }
        [BsonElement("message")]
        [BsonRepresentation(BsonType.String)]
        [Required]
        public string Message { get; set; }
        [BsonElement("date")]
        [Required]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

        //[BsonElement("exception")]
        //public Exception Exception { get; set; }

        public LoggerModel()
        {
        }

        public LoggerModel(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, string message)
        {
            Id = Guid.NewGuid().ToString("N");
            LogLevel = logLevel;
            EventId = eventId;
            Message = message;
            Date = DateTime.Now;
        }
    }
}
