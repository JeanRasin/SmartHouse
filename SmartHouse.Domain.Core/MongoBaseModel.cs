using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    public class MongoBaseModel
    {
        [Column("_id")]
        public string Id { get; set; }
    }
}
