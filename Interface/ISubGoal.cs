using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Interface
{
    public interface ISubGoal
    {
       Task< List<Ref_SubGoals>> GetSubGoalAsync();
    }
}
