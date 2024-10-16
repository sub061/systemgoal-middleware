using System;

namespace DataHub_System_Goal.Models
{
    public class RefKPI
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public string Title { get; set; }
        public int SubGoalId { get; set; }
        public string Sitelevel { get; set; }
        public string ValueType { get; set; }
        public string? KPI_DEFINATION { get; set; }
    }
}
