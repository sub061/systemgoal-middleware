using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Interface
{
    public interface ISummary
    {
       Task< List<Ref_Summary>> GetSummaryAsync();
        Task PutSummaryAsync( List<Ref_Summary> summary);
        Task<List<Ref_Summary>> GetSummaryForFormAsync();
        Task<List<Ref_PDF>> GetPDF(int[] hospitals, int[] pillars);
    }
}
