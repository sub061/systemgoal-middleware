using System.Data;
using System.Data.SqlClient;
using Datahub_System_Goal.Service;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Models;
using Snowflake.Data.Client;

namespace DataHub_System_Goal.Repository
{
    public class HospitalRepository : IHospital
    {
        private readonly IConfiguration _configuration;
        private readonly SnowflakeService _snowflakeService; 

        public HospitalRepository(IConfiguration configuration , SnowflakeService snowflakeService)
        {
            _configuration = configuration;
            _snowflakeService = snowflakeService;
        }



        public async Task<RefOperatingModel> GetOperatingModelAsync()
        {
            RefOperatingModel operatingModel = new RefOperatingModel();

            DataTable dt = await _snowflakeService.GetDataFromTableAsync("REF_OPERATING_MODEL");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                operatingModel.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                operatingModel.Title = dt.Rows[i]["OPERATING_MODEL_TITLE"].ToString();
                
            }

            return operatingModel;



        }


        public async Task< List<RefHospital>> GetHospitalAsync()
        {
            List<RefHospital> listHospital = new List<RefHospital>();

            DataTable dt = await _snowflakeService.GetDataFromTableAsync("REF_HOSPITAL");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RefHospital hospital = new RefHospital();
                hospital.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                hospital.Title = dt.Rows[i]["HOSPITAL_NAME"].ToString();
                hospital.DivisionId = dt.Rows[i]["DIVISION_ID"] !=
                                      DBNull.Value
                    ? Convert.ToInt32(dt.Rows[i]["DIVISION_ID"])
                    : null;

                hospital.OrganizationId = dt.Rows[i]["ORGANIZATION_ID"] !=
                                          DBNull.Value
                    ? Convert.ToInt32(dt.Rows[i]["ORGANIZATION_ID"])
                    : null;
                listHospital.Add(hospital);
            }
           
            return listHospital;



        }



        //public List<RefHospital> GetHospital()
        //{
        //    List<RefHospital> listHospital = new List<RefHospital>();
        //    IDbConnection conn = new SnowflakeDbConnection(_configuration.GetConnectionString("DBConnection"));
        //    DataTable dt = new DataTable();
        //    IDbCommand command = new ("select * from REF_HOSPITAL", conn);
        //    conn.Open();
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    adapter.Fill(dt);

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        RefHospital hospital = new RefHospital();
        //        hospital.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
        //        hospital.Title = dt.Rows[i]["HOSPITAL_NAME"].ToString();
        //        hospital.DivisionId = dt.Rows[i]["DIVISION_ID"] !=
        //                              DBNull.Value
        //            ? Convert.ToInt32(dt.Rows[i]["DIVISION_ID"])
        //            : null;

        //        hospital.OrganizationId = dt.Rows[i]["ORGANIZATION_ID"] !=
        //                                  DBNull.Value
        //            ? Convert.ToInt32(dt.Rows[i]["ORGANIZATION_ID"])
        //            : null;
        //        listHospital.Add(hospital);
        //    }
        //    conn.Close();
        //    return listHospital;



        //}
    }
}
