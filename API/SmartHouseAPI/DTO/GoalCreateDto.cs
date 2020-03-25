using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.InputModel
{
    public class GoalCreateDto
    {
        [Required(ErrorMessage = "Name not null or empty.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The length of the test should be from 4 to 100")]
        public string Name { get; set; }

        public GoalCreateDto()
        {
        }

        public GoalCreateDto(string name)
        {
            Name = name;
        }
    }
}