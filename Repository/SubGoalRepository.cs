using System.Data;
using System.Data.SqlClient;
using Datahub_System_Goal.Service;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Models;

namespace DataHub_System_Goal.Repository
{
    public class SubGoalRepository : ISubGoal
    {
        private readonly IConfiguration _configuration;
        private readonly SnowflakeService _snowflakeService;

        public SubGoalRepository(IConfiguration configuration, SnowflakeService snowflakeService)
        {
            _configuration = configuration;
            _snowflakeService = snowflakeService;
        }
        public async Task<List<Ref_SubGoals>> GetSubGoalAsync()
        {
            List<Ref_SubGoals> listSubGoals = new List<Ref_SubGoals>();
            DataTable dt = await _snowflakeService.GetDataFromTableAsync("REF_SUB_GOAL");
            //SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DbConnection"));
            //DataTable dt = new DataTable();
            //SqlCommand command = new SqlCommand("select * from REF_SUB_GOAL", conn);
            //conn.Open();
            //SqlDataAdapter adapter = new SqlDataAdapter(command);
            //adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Ref_SubGoals subGoal = new Ref_SubGoals();
                subGoal.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                subGoal.Title = dt.Rows[i]["SUB_GOAL_TITLE"].ToString();
                subGoal.PillarId = Convert.ToInt32(dt.Rows[i]["PILLAR_ID"]);

                listSubGoals.Add(subGoal);
            }
            return listSubGoals;
        }
    }
}
