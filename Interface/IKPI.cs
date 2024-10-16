using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Interface
{
    public interface IKPI
    {
        Task<List<RefKPI>> GetKPIAsync();
    }
}
