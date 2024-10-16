using System.Data;
using System.Data.SqlClient;
using Datahub_System_Goal.Service;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Repository
{
    public class KPIRepository : IKPI
    {
        private readonly IConfiguration _configuration;
        private readonly SnowflakeService _snowflakeService;

        public KPIRepository(IConfiguration configuration , SnowflakeService snowflakeService)
        {
            _configuration = configuration;
            _snowflakeService = snowflakeService;
        }

        public async Task<List<RefKPI>> GetKPIAsync()
        {
            List<RefKPI> listKpi = new List<RefKPI>();
           
            DataTable dt = await _snowflakeService.GetDataFromTableAsync("REF_KPI");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RefKPI kpi = new RefKPI();
                kpi.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                kpi.GoalId = Convert.ToInt32(dt.Rows[i]["PILLAR_ID"]); //TODO : Add new 
                kpi.Title = dt.Rows[i]["KPI_TITLE"].ToString();
                kpi.SubGoalId = Convert.ToInt32(dt.Rows[i]["SUB_GOAL_ID"]);
                kpi.Sitelevel = dt.Rows[i]["ENTITY_LEVEL"].ToString();
                kpi.KPI_DEFINATION = dt.Rows[i]["KPI_DEFINATION"].ToString();
                kpi.ValueType = dt.Rows[i]["VALUE_TYPE"].ToString();
                listKpi.Add(kpi);
            }

            return listKpi;
        }
    }
}
