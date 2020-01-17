using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("Logger")]
    public class LoggerModel : MongoBaseModel
    {
        [Column("message")]
        public string Message { get; set; }

        public LoggerModel(string message)
        {
            Message = message;
        }
    }
}
