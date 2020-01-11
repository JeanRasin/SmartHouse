using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("logger")]
    public class Logger : MongoBase
    {
        [BsonElement("logLevel")]
        [BsonRepresentation(BsonType.Int32)]
        public Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; }

        [BsonElement("eventId")]
        [Required]
        [JsonProperty(Required = Required.Always)]
        public EventId EventId { get; set; }

        [BsonElement("message")]
        [BsonRepresentation(BsonType.String)]
        [Required]
        [JsonProperty(Required = Required.AllowNull)]
        public string Message { get; set; }

        [BsonElement("date")]
        [Required]
        [BsonRepresentation(BsonType.DateTime)]
        [JsonProperty(Required = Required.Always)]
        public DateTime Date { get; set; }

        //[BsonElement("exception")]
        //public Exception Exception { get; set; }

        public Logger()
        {
        }

        public Logger(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, string message)
        {
            Id = Guid.NewGuid().ToString("N");
            LogLevel = logLevel;
            EventId = eventId;
            Message = message;
            Date = DateTime.Now;
        }
    }
}