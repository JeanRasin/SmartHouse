using System.ComponentModel.DataAnnotations;

namespace SmartHouseAPI.InputModel
{
    public class GoalCreateInput
    {
        [Required(ErrorMessage = "Name not null or empty.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The length of the test should be from 0 to 100")]
        public string Name { get; set; }

        public GoalCreateInput()
        {
        }

        public GoalCreateInput(string name)
        {
            Name = name;
        }
    }
}
