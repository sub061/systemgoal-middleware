using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Datahub_System_Goal.Service;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Models;
using Snowflake.Data.Client;

namespace DataHub_System_Goal.Repository
{
    public class SummaryRepository : ISummary
    {
        private readonly SnowflakeDbConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly SnowflakeService _snowflakeService;

        public SummaryRepository(IConfiguration configuration, SnowflakeDbConnection snowflakeDbConnection , SnowflakeService snowflakeService)
        {
            _configuration = configuration;
            _connection = snowflakeDbConnection;
            _snowflakeService = snowflakeService;
        }

        public async Task PutSummaryAsync(List<Ref_Summary> summarys)
        {
            try
            {
                await _connection.OpenAsync();

                foreach (var summary in summarys)
                {


                    using (SnowflakeDbCommand cmd = (SnowflakeDbCommand)_connection.CreateCommand())
                    {
                      
                        string utcNowString = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        DateTime estTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, estZone);
                        string estNowString = estTime.ToString("yyyy-MM-dd HH:mm:ss");

                        string query = "UPDATE KPI_ACTUAL_TARGET_INPUT " +
    "SET MTD_QTD_ACTUAL = '" + summary.MTD_ACTUAL +
    "', MTD_BUDGET_TARGET = '" + summary.MTD_BUDGET +
    "', MTD_PRIOR_YEAR ='" + summary.MTD_PRIOR_YEAR +
    "', YTD_ACTUAL ='" + summary.YTD_ACTUAL +
    "', YTD_BUDGET_TARGET = '" + summary.YTD_BUDGET +
    "', YTD_PRIOR_YEAR ='" + summary.YTD_PRIOR_YEAR +
    "', MONTH_QUARTER ='" + summary.ReportType +
     "', COMMENTS ='" + summary.Comment +
    "', URL = '" + summary.URL +
    "', REC_MODIFY_DT = '"+ estNowString +
    "', REC_MODIFY_BY = '" + summary.REC_MODIFY_BY +
    "' WHERE HOSPITAL_ID = " + summary.HospitalId +
    " AND KPI_ID = " + summary.KPIId +
    " AND OPERATING_MODEL_ID = " + summary.OperatingModelId;

                        cmd.CommandText = query;


                        // Execute the update command
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        // Command to execute the stored procedure
                 


                    }

                 
                }

                using (SnowflakeDbCommand cmd2 = (SnowflakeDbCommand)_connection.CreateCommand())
                {
                    cmd2.CommandType = CommandType.Text; // Keep as Text, Snowflake uses SQL-like syntax for stored procedures
                    cmd2.CommandText = "CALL datahub_system_goals.system_goals.uspSummary_Report_Variance()";  // Call stored procedure

                    // Execute the procedure
                    using (SnowflakeDbDataReader reader = (SnowflakeDbDataReader)await cmd2.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Process the results if the procedure returns any
                            Console.WriteLine(reader[0]); // Example: output the first column of the result
                        }
                    }
                }


                // Close the connection
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           


        }

        public async Task<List<Ref_PDF>> GetPDF(int[] hospitals , int[] pillars)
        {
            List<Ref_PDF> listSummary = new List<Ref_PDF>();
            DataTable dt = new DataTable();
            // Parameterize the query to avoid SQL injection
            var hospitalsParam = string.Join(",", hospitals);
            var pillarsParam = string.Join(",", pillars);

            await _connection.OpenAsync();
            using (SnowflakeDbCommand cmd = (SnowflakeDbCommand)_connection.CreateCommand())
            {
                cmd.CommandText = $" SELECT" +
                    $" OPERATING_MODEL_ID," +
                    $"REF_KPI.SUB_GOAL_ID,"+
                    $"REF_PILLARS.ID as PILLARS_ID,"+
                    $" OPERATING_MODEL, REF_PILLARS.PILLAR_NAME," +
                    $" SUB_GOAL_TITLE, KPI_ID," +
                    $" REF_KPI.ENTITY_LEVEL," +
                    $" KPI_REPORTING_SUMMARY.KPI_TITLE," +
                    $"KPI_REPORTING_SUMMARY.YTD_PRIOR_YEAR_VAR_SIGN," +
                    $"KPI_REPORTING_SUMMARY.MTD_PRIOR_YEAR_VAR_SIGN ," +
                    $"KPI_REPORTING_SUMMARY.YTD_BUDGET_VAR_SIGN," +
                    $"KPI_REPORTING_SUMMARY.MTD_BUDGET_VAR_SIGN ," +
                       $"MTD_BUDGET_VAR_CLR , " +
                    $"MTD_PRIOR_YEAR_VAR_CLR , " +
                    $"YTD_BUDGET_VAR_CLR , " +
                    $"YTD_PRIOR_YEAR_VAR_CLR , " +
                    $" HOSPITAL_ID," +
                    $" HOSPITAL_NAME," +
                    $" MONTH_QUARTER," +
                    $" KPI_REPORTING_SUMMARY.MTD_QTD_ACTUAL," +
                    $" KPI_REPORTING_SUMMARY.MTD_BUDGET_TARGET," +
                    $" KPI_REPORTING_SUMMARY.MTD_BUDGET_VARIANCE," +
                    $" KPI_REPORTING_SUMMARY.MTD_PRIOR_YEAR," +
                    $" MTD_PRIOR_YEAR_VARIANCE," +
                    $" KPI_REPORTING_SUMMARY.YTD_ACTUAL," +
                    $" KPI_REPORTING_SUMMARY.YTD_BUDGET_TARGET," +
                    $" KPI_REPORTING_SUMMARY.YTD_BUDGET_VARIANCE," +
                    $" KPI_REPORTING_SUMMARY.YTD_PRIOR_YEAR," +
                    $" YTD_PRIOR_YEAR_VARIANCE" +
                    $" FROM " +
                    $" (KPI_REPORTING_SUMMARY  INNER JOIN REF_KPI ON KPI_REPORTING_SUMMARY.KPI_ID = REF_KPI. ID)" +
                    $" INNER JOIN REF_PILLARS ON REF_PILLARS.ID = REF_KPI.PILLAR_ID" +
                    $" Where HOSPITAL_ID IN ("+ hospitalsParam + ") and PILLAR_ID IN ("+ pillarsParam + ") order by REF_PILLARS.ID asc, KPI_ID asc , HOSPITAL_ID asc "; 

                using (var adapter = new SnowflakeDbDataAdapter(cmd))
                {

                    adapter.Fill(dt);

                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Ref_PDF summary = new Ref_PDF();

               // summary.Comment = dt.Rows[i]["COMMENTS"].ToString();
                //summary.URL = dt.Rows[i]["URL"].ToString();
                summary.HOSPITAL_ID = Convert.ToInt32(dt.Rows[i]["HOSPITAL_ID"]);
                summary.PILLARS_ID = Convert.ToInt32(dt.Rows[i]["PILLARS_ID"]);
                summary.SUB_GOAL_ID = Convert.ToInt32(dt.Rows[i]["SUB_GOAL_ID"]);
                summary.KPI_ID = Convert.ToInt32(dt.Rows[i]["KPI_Id"]);
                summary.OPERATING_MODEL_ID = Convert.ToInt32(dt.Rows[i]["OPERATING_MODEL_ID"]);
             
                //summary.MTD_BUDGET_VAR_SIGN = dt.Rows[i]["MTD_BUDGET_VAR_SIGN"].ToString();
                //summary.MTD_PRIOR_YEAR_VAR_SIGN = dt.Rows[i]["MTD_PRIOR_YEAR_VAR_SIGN"].ToString();
                //summary.YTD_BUDGET_VAR_SIGN = dt.Rows[i]["YTD_BUDGET_VAR_SIGN"].ToString();
                //summary.YTD_PRIOR_YEAR_VAR_SIGN = dt.Rows[i]["YTD_PRIOR_YEAR_VAR_SIGN"].ToString();
                
                summary.PILLAR_NAME = dt.Rows[i]["PILLAR_NAME"].ToString();
                summary.OPERATING_MODEL = dt.Rows[i]["OPERATING_MODEL"].ToString();
                summary.SUB_GOAL_TITLE = dt.Rows[i]["SUB_GOAL_TITLE"].ToString();
                summary.ENTITY_LEVEL = dt.Rows[i]["ENTITY_LEVEL"].ToString();
                summary.KPI_TITLE = dt.Rows[i]["KPI_TITLE"].ToString();
                summary.HOSPITAL_NAME = dt.Rows[i]["HOSPITAL_NAME"].ToString();
                summary.MONTH_QUARTER = dt.Rows[i]["MONTH_QUARTER"].ToString();
                summary.MTD_QTD_ACTUAL = dt.Rows[i]["MTD_QTD_ACTUAL"].ToString();
                summary.MTD_BUDGET_TARGET = dt.Rows[i]["MTD_BUDGET_TARGET"].ToString();
                summary.MTD_PRIOR_YEAR = dt.Rows[i]["MTD_PRIOR_YEAR"].ToString();
                summary.MTD_BUDGET_VARIANCE = dt.Rows[i]["MTD_BUDGET_VARIANCE"].ToString();
                summary.MTD_PRIOR_YEAR_VARIANCE = dt.Rows[i]["MTD_PRIOR_YEAR_VARIANCE"].ToString();
                summary.YTD_ACTUAL = dt.Rows[i]["YTD_ACTUAL"].ToString();
                summary.YTD_BUDGET_TARGET = dt.Rows[i]["YTD_BUDGET_TARGET"].ToString();
                summary.YTD_PRIOR_YEAR = dt.Rows[i]["YTD_PRIOR_YEAR"].ToString();
                
                
                summary.YTD_BUDGET_VARIANCE = dt.Rows[i]["YTD_BUDGET_VARIANCE"].ToString();
                summary.YTD_PRIOR_YEAR_VARIANCE = dt.Rows[i]["YTD_PRIOR_YEAR_VARIANCE"].ToString();



                summary.MTD_BUDGET_VAR_SIGN = dt.Rows[i]["MTD_BUDGET_VAR_CLR"].ToString();
                summary.MTD_PRIOR_YEAR_VAR_SIGN = dt.Rows[i]["MTD_PRIOR_YEAR_VAR_CLR"].ToString();
                summary.YTD_BUDGET_VAR_SIGN = dt.Rows[i]["YTD_BUDGET_VAR_CLR"].ToString();
                summary.YTD_PRIOR_YEAR_VAR_SIGN = dt.Rows[i]["YTD_PRIOR_YEAR_VAR_CLR"].ToString();



                //summary.MTD_BUDGET_VAR_CLR = dt.Rows[i]["MTD_BUDGET_VAR_CLR"].ToString();
                //summary.MTD_PRIOR_YEAR_VAR_CLR = dt.Rows[i]["MTD_PRIOR_YEAR_VAR_CLR"].ToString();
                //summary.YTD_BUDGET_VAR_CLR = dt.Rows[i]["YTD_BUDGET_VAR_CLR"].ToString();
                //summary.YTD_PRIOR_YEAR_VAR_CLR = dt.Rows[i]["YTD_PRIOR_YEAR_VAR_CLR"].ToString();

                listSummary.Add(summary);

            }

            return listSummary;


        }

        public async Task<List<Ref_Summary>> GetSummaryAsync()
        {
            List<Ref_Summary> listSummary = new List<Ref_Summary>();
            // DataTable dt = await _snowflakeService.GetDataFromTableAsync("KPI_ACTUAL_TARGET_INPUT");

            DataTable dt = new DataTable();

            await _connection.OpenAsync();
            using (SnowflakeDbCommand cmd = (SnowflakeDbCommand)_connection.CreateCommand())
            {
                cmd.CommandText = $" select  b.id as KPIId ," +
                    $" a.HOSPITAL_ID as HospitalId, '1' as   OperatingModelId ," +
                    $" d.id as GoalId, c.id as SubGoalId," +
                    $"a.YTD_PRIOR_YEAR_VAR_SIGN,"+
                    $"a.MTD_PRIOR_YEAR_VAR_SIGN ,"+
                    $"YTD_BUDGET_VAR_SIGN,"+
                    $"b.KPI_DEFINATION,"+
                    $"MTD_BUDGET_VAR_SIGN , "+

                    $"MTD_BUDGET_VAR_CLR , " +
                    $"MTD_PRIOR_YEAR_VAR_CLR , " +
                    $"YTD_BUDGET_VAR_CLR , " +
                    $"YTD_PRIOR_YEAR_VAR_CLR , " +


                    $"a.URL,a.COMMENTS,"+
                    $"b.VALUE_TYPE as ValueType,"+
                     $"a.MONTH_QUARTER as  ReportType," +
                     $"a.MTD_QTD_ACTUAL as MTD_ACTUAL," +
                     $"a.MTD_BUDGET_TARGET as MTD_BUDGET," +
                     $"a.MTD_PRIOR_YEAR as MTD_PRIOR_YEAR," +
                     $"a.YTD_ACTUAL as YTD_ACTUAL," +
                     $"a.YTD_BUDGET_TARGET as YTD_BUDGET," +
                     $"a.YTD_PRIOR_YEAR,a.MTD_BUDGET_VARIANCE as MTD_BUDGET_VARIANCE ,a.MTD_PRIOR_YEAR_VARIANCE as MTD_PRIOR_YEAR_VARIANCE," +
                     $"a.YTD_BUDGET_VARIANCE as YTD_BUDGET_VARIANCE,a.YTD_PRIOR_YEAR_VARIANCE as YTD_PRIOR_YEAR_VARIANCE from KPI_REPORTING_SUMMARY " +
                      $"a join REF_KPI b on a.kpi_id = b.id join ref_sub_goal c on b.sub_goal_id = c.id join ref_pillars d on c.pillar_id = d.id";


                using (var adapter = new SnowflakeDbDataAdapter(cmd))
                {
                   
                    adapter.Fill(dt);
                  
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Ref_Summary summary = new Ref_Summary();
                //summary.Comment = dt.Rows[i]["COMMENT"].ToString();
                summary.Comment = dt.Rows[i]["COMMENTS"].ToString();
                summary.URL = dt.Rows[i]["URL"].ToString();
                summary.HospitalId = Convert.ToInt32(dt.Rows[i]["HospitalId"]);
                summary.KPIId = Convert.ToInt32(dt.Rows[i]["KPIId"]);
                //summary.OperatingModelId = Convert.ToInt32(dt.Rows[i]["OPERATING_MODEL_ID"]);
                summary.OperatingModelId = 1;
                summary.GoalId = Convert.ToInt32(dt.Rows[i]["GoalId"]);
                summary.SubGoalId = Convert.ToInt32(dt.Rows[i]["SubGoalId"]);
                summary.ReportType = dt.Rows[i]["ReportType"].ToString();
                summary.MTD_ACTUAL = dt.Rows[i]["MTD_ACTUAL"].ToString();
                summary.KPI_DEFINATION = dt.Rows[i]["KPI_DEFINATION"].ToString() ;


                summary.MTD_BUDGET = dt.Rows[i]["MTD_BUDGET"].ToString();
                summary.MTD_PRIOR_YEAR = dt.Rows[i]["MTD_PRIOR_YEAR"].ToString();
                summary.MTD_BUDGET_VARIANCE = dt.Rows[i]["MTD_BUDGET_VARIANCE"].ToString();
                summary.MTD_PRIOR_YEAR_VARIANCE = dt.Rows[i]["MTD_PRIOR_YEAR_VARIANCE"].ToString();

                summary.MTD_BUDGET_VAR_SIGN = dt.Rows[i]["MTD_BUDGET_VAR_SIGN"].ToString();
                summary.MTD_PRIOR_YEAR_VAR_SIGN = dt.Rows[i]["MTD_PRIOR_YEAR_VAR_SIGN"].ToString();
                summary.YTD_BUDGET_VAR_SIGN = dt.Rows[i]["YTD_BUDGET_VAR_SIGN"].ToString();
                summary.YTD_PRIOR_YEAR_VAR_SIGN = dt.Rows[i]["YTD_PRIOR_YEAR_VAR_SIGN"].ToString();



                summary.MTD_BUDGET_VAR_CLR = dt.Rows[i]["MTD_BUDGET_VAR_CLR"].ToString();
                summary.MTD_PRIOR_YEAR_VAR_CLR = dt.Rows[i]["MTD_PRIOR_YEAR_VAR_CLR"].ToString();
                summary.YTD_BUDGET_VAR_CLR = dt.Rows[i]["YTD_BUDGET_VAR_CLR"].ToString();
                summary.YTD_PRIOR_YEAR_VAR_CLR = dt.Rows[i]["YTD_PRIOR_YEAR_VAR_CLR"].ToString();



                summary.YTD_ACTUAL = dt.Rows[i]["YTD_ACTUAL"].ToString();
                summary.YTD_BUDGET = dt.Rows[i]["YTD_BUDGET"].ToString();
                summary.YTD_PRIOR_YEAR = dt.Rows[i]["YTD_PRIOR_YEAR"].ToString();
                summary.YTD_BUDGET_VARIANCE = dt.Rows[i]["YTD_BUDGET_VARIANCE"].ToString();
                summary.YTD_PRIOR_YEAR_VARIANCE = dt.Rows[i]["YTD_PRIOR_YEAR_VARIANCE"].ToString();
                summary.ValueType = dt.Rows[i]["ValueType"].ToString();
                listSummary.Add(summary);

            }

            return listSummary;
        }

        public async Task<List<Ref_Summary>> GetSummaryForFormAsync()
        {
            List<Ref_Summary> listSummary = new List<Ref_Summary>();
            // DataTable dt = await _snowflakeService.GetDataFromTableAsync("KPI_ACTUAL_TARGET_INPUT");

            DataTable dt = new DataTable();

            await _connection.OpenAsync();
            using (SnowflakeDbCommand cmd = (SnowflakeDbCommand)_connection.CreateCommand())
            {
                cmd.CommandText = $" select   b.id as KPIId , a.HOSPITAL_ID as HospitalId, '1' as   OperatingModelId ,d.id as GoalId,c.id as SubGoalId,b.VALUE_TYPE as ValueType," +
                    $"a.MONTH_QUARTER as  ReportType,a.MTD_QTD_ACTUAL as MTD_ACTUAL," +
                          $"a.URL,a.COMMENTS, b.ENTITY_LEVEL ," +
                    $"a.MTD_BUDGET_TARGET as MTD_BUDGET,a.MTD_PRIOR_YEAR as MTD_PRIOR_YEAR," +
                    $"a.YTD_ACTUAL as YTD_ACTUAL,a.YTD_BUDGET_TARGET as YTD_BUDGET," +
                    $"a.YTD_PRIOR_YEAR from KPI_ACTUAL_TARGET_INPUT a " +
                    $"join REF_KPI b on a.kpi_id = b.id join ref_sub_goal c on b.sub_goal_id = c.id join ref_pillars d on c.pillar_id = d.id";
                using (var adapter = new SnowflakeDbDataAdapter(cmd))
                {

                    adapter.Fill(dt);

                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Ref_Summary summary = new Ref_Summary();
                //summary.Comment = dt.Rows[i]["COMMENT"].ToString();
                summary.HospitalId = Convert.ToInt32(dt.Rows[i]["HospitalId"]);
                summary.KPIId = Convert.ToInt32(dt.Rows[i]["KPIId"]);
                //summary.OperatingModelId = Convert.ToInt32(dt.Rows[i]["OPERATING_MODEL_ID"]);
                summary.OperatingModelId = 1;
                summary.GoalId = Convert.ToInt32(dt.Rows[i]["GoalId"]);
                summary.SubGoalId = Convert.ToInt32(dt.Rows[i]["SubGoalId"]);
                summary.Comment = dt.Rows[i]["COMMENTS"].ToString();
                summary.URL = dt.Rows[i]["URL"].ToString();
                summary.ReportType = dt.Rows[i]["ReportType"].ToString();
                summary.MTD_ACTUAL = dt.Rows[i]["MTD_ACTUAL"].ToString();

                summary.ENTITY_LEVEL = dt.Rows[i]["ENTITY_LEVEL"].ToString();
                summary.IsAvaliable = dt.Rows[i]["ENTITY_LEVEL"].ToString() == "Y"
     ? true
     : dt.Rows[i]["ENTITY_LEVEL"].ToString() == "N" && Convert.ToInt32(dt.Rows[i]["HospitalId"]) == 22
         ? true
         : dt.Rows[i]["ENTITY_LEVEL"].ToString() == "N" && Convert.ToInt32(dt.Rows[i]["HospitalId"]) != 22
             ? false
             : default(bool);

                summary.MTD_BUDGET = dt.Rows[i]["MTD_BUDGET"].ToString();
                summary.MTD_PRIOR_YEAR = dt.Rows[i]["MTD_PRIOR_YEAR"].ToString();
                //summary.MTD_BUDGET_VARIANCE = dt.Rows[i]["MTD_BUDGET_VARIANCE"].ToString();
                //summary.MTD_PRIOR_YEAR_VARIANCE = dt.Rows[i]["MTD_PRIOR_YEAR_VARIANCE"].ToString();

                summary.YTD_ACTUAL = dt.Rows[i]["YTD_ACTUAL"].ToString();
                summary.YTD_BUDGET = dt.Rows[i]["YTD_BUDGET"].ToString();
                summary.YTD_PRIOR_YEAR = dt.Rows[i]["YTD_PRIOR_YEAR"].ToString();
                //summary.YTD_BUDGET_VARIANCE = dt.Rows[i]["YTD_BUDGET_VARIANCE"].ToString();
                //summary.YTD_PRIOR_YEAR_VARIANCE = dt.Rows[i]["YTD_PRIOR_YEAR_VARIANCE"].ToString();
                summary.ValueType = dt.Rows[i]["ValueType"].ToString();
                listSummary.Add(summary);

            }

            return listSummary;
        }


     

    }
}
