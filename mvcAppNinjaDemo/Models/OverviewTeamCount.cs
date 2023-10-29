namespace mvcAppNinjaDemo.Models
{
    public class OverviewTeamCount
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int NinjaCount { get; set; }

        public bool HasMission { get; set; }

        public string RankMission { get; set;}

    }
}
