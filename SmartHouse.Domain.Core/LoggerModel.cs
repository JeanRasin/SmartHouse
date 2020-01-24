using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("logger")]
    public class LoggerModel : MongoBaseModel
    {
        [BsonElement("logLevel")]
        public Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; }

        [BsonElement("eventId")]
        public EventId EventId { get; set; }
        [BsonElement("message")]
        public string Message { get; set; }

        //[BsonElement("exception")]
        //public Exception Exception { get; set; }

        public LoggerModel()
        {
        }

        public LoggerModel(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, string message)
        {
            LogLevel = logLevel;
            EventId = eventId;
            Message = message;
        }
    }
}
