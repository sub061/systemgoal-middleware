using System.Data;
using System.Data.SqlClient;
using Datahub_System_Goal.Service;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Repository
{
    public class PillarRepository : IPillar
    {
        private readonly IConfiguration _configuration;
        private readonly SnowflakeService _snowflakeService;

        public PillarRepository(IConfiguration configuration, SnowflakeService snowflakeService)
        {
            _configuration = configuration;
            _snowflakeService = snowflakeService;
        }

        public async Task<List<Ref_Pillars>> GetPillarAsync()
        {
            List<Ref_Pillars> listPillar = new List<Ref_Pillars>();
            DataTable dt = await _snowflakeService.GetDataFromTableAsync("REF_Pillars");



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Ref_Pillars pillars = new Ref_Pillars();
                pillars.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                pillars.Title = dt.Rows[i]["PILLAR_NAME"].ToString();

                listPillar.Add(pillars);

            }
            return listPillar;
        }
    }
}
