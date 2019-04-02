namespace MOE.Common.Models.ViewModel
{
    public class Day
    {
        public Day(int id, string name)
        {
            DayId = id;
            Name = name;
        }

        public int DayId { get; set; }
        public string Name { get; set; }
    }
}