using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("Goal")]
    public class Goal
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
