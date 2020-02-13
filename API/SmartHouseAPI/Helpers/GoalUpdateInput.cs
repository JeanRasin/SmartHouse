using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.Helpers
{
    public class GoalUpdateInput : GoalCreateInput
    {
        [Required(ErrorMessage = "Id not null or empty.")]
        public Guid Id { get; set; }

        public GoalUpdateInput()
        {
        }

        public GoalUpdateInput(Guid id = default, string name = null)
        {
            Id = id;
            Name = name;
        }
    }
}
