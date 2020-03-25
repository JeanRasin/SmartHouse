using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.InputModel
{
    public class GoalUpdateDto : GoalCreateDto
    {
        [Required(ErrorMessage = "Id not null or empty.")]
        public Guid Id { get; set; }

        public bool Done { get; set; }

        public GoalUpdateDto()
        {
        }

        public GoalUpdateDto(Guid id = default, string name = null, bool done = false)
        {
            Id = id;
            Name = name;
            Done = done;
        }
    }
}