namespace SmartHouseAPI.ViewModel
{
    public class GoalCreateInput
    {
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
