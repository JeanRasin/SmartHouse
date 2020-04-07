using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.InputModel
{
    public class GoalDoneDto
    {
        [Required(ErrorMessage = "Id not null or empty.")]
        public Guid Id { get; set; }

        public bool Done { get; set; }

        public GoalDoneDto()
        {
        }

        public GoalDoneDto(Guid id, bool done)
        {
            Id = id;
            Done = done;
        }
    }
}