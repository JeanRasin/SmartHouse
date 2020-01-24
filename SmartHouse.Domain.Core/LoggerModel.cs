using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("logger")]
    public class LoggerModel : MongoBaseModel
    {
        [BsonElement("message")]
        public string Message { get; set; }

        public LoggerModel()
        {
        }

        public LoggerModel(string message)
        {
            Message = message;
        }
    }
}
