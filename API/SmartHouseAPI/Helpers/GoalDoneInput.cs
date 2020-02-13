using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.Helpers
{
    public class GoalDoneInput
    {
        [Required(ErrorMessage = "Id not null or empty.")]
        public Guid Id { get; set; }
        public bool Done { get; set; }

        public GoalDoneInput()
        {

        }

        public GoalDoneInput(Guid id, bool done)
        {
            Id = id;
            Done = done;
        }

    }
}
