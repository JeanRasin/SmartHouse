using System;

namespace SmartHouseAPI.ViewModel
{
    public class GoalUpdateInput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

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
