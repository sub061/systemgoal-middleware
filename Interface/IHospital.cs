using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Interface
{
    public interface IHospital
    {
        Task<RefOperatingModel> GetOperatingModelAsync();
        Task<List<RefHospital>> GetHospitalAsync();
    }
    
}
