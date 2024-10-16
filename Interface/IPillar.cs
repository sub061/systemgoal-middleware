using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Interface
{
    public interface IPillar
    {
        Task< List<Ref_Pillars>> GetPillarAsync();
    }
}
