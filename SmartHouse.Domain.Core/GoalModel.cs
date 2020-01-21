using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("Goal")]
    public class GoalModel
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
