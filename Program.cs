
using Datahub_System_Goal.Service;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Repository;
using Snowflake.Data.Client;

namespace Datahub_System_Goal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton( DbConnection =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DbConnection");
                var conn = new SnowflakeDbConnection();
                conn.ConnectionString = connectionString;
                return conn;
            });

            builder.Services.AddSingleton<SnowflakeService>();
            builder.Services.AddSingleton<IHospital, HospitalRepository>();
            builder.Services.AddSingleton<IKPI, KPIRepository>();
            builder.Services.AddSingleton<IPillar, PillarRepository>();
            builder.Services.AddSingleton<ISubGoal, SubGoalRepository>();
            builder.Services.AddSingleton<ISummary, SummaryRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
