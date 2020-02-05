using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("Goal")]
    public class GoalModel
    {
        [Key]
        [Column("Id", Order = 0)]
        public Guid Id { get; set; }
        [Column("Name", Order = 1)]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        [Column("Date_Create", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreate { get; set; } = DateTime.UtcNow;
        [Required]
        [Column("Date_Create", Order = 3)]
        public DateTime DateUpdate { get; set; }
        [Required]
        [Column("Date_Create", Order = 4)]
        public bool Done { get; set; }

        public GoalModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            DateCreate = DateTime.UtcNow;
            DateUpdate = DateTime.UtcNow;
            Done = false;
        }
    }
}
