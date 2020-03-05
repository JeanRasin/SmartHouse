using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.InputModel
{
    public class GoalUpdateInput : GoalCreateInput
    {
        [Required(ErrorMessage = "Id not null or empty.")]
        public Guid Id { get; set; }
        public bool Done { get; set; }

        public GoalUpdateInput()
        {
        }

        public GoalUpdateInput(Guid id = default, string name = null, bool done = false)
        {
            Id = id;
            Name = name;
            Done = done;
        }
    }
}
