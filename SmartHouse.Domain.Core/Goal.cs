using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse.Domain.Core
{
    [Table("Goal")]
    public class Goal
    {
        [Key]
        [Column("Id", Order = 0)]
        [JsonProperty(Required = Required.Always)]
        public Guid Id { get; set; }

        [Column("Name", Order = 1)]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [Required]
        [Column("Date_Create", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(Required = Required.Always)]
        public DateTime DateCreate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("Date_Update", Order = 3)]
        [JsonProperty(Required = Required.Always)]
        public DateTime DateUpdate { get; set; }

        [Required]
        [Column("Done", Order = 4)]
        [JsonProperty(Required = Required.Always)]
        public bool Done { get; set; }

        public Goal()
        {
        }

        public Goal(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            DateCreate = DateTime.UtcNow;
            DateUpdate = DateTime.UtcNow;
            Done = false;
        }
    }
}