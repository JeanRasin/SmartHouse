using System;

namespace SmartHouseAPI.Helpers
{
    public class GoalDoneInput
    {
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
