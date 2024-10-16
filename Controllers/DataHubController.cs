using Amazon.Runtime.Internal.Auth;
using Datahub_System_Goal;
using DataHub_System_Goal.Interface;
using DataHub_System_Goal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataHub_System_Goal.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataHubController : ControllerBase
    {
        private readonly IHospital _hospital;
        private readonly IKPI _kpi;
        private readonly IPillar _pillar;
        private readonly ISubGoal _subGoal;
        private readonly ISummary _summary;

        public DataHubController(IHospital hospital, IKPI kpi, IPillar pillar, ISubGoal subGoal, ISummary summary)
        {
            _hospital = hospital;
            _kpi = kpi;
            _pillar = pillar;
            _subGoal = subGoal;
            _summary = summary;
        }

        [HttpGet]
        [Route("hospitals")]
        public async Task<IActionResult> GetHospital()
        {
          var result =  await _hospital.GetHospitalAsync();
          return Ok(result);

        }

        [HttpGet]
        [Route("operatingmodel")]
        public async Task<IActionResult> GetModel()
        {
            var result = await _hospital.GetOperatingModelAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("kpis")]
        public async Task<IActionResult> GetKPI()
        {
            var result = await _kpi.GetKPIAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("pillers")]
        public async Task<IActionResult> GetPillar()
        {
            var result = await _pillar.GetPillarAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("subgoals")]
        public async Task<IActionResult> GetSubGoals()
        {
            var result =await _subGoal.GetSubGoalAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _summary.GetSummaryAsync();
            return Ok(result);
        }
        [Route("summary")]
        [HttpPut]
        public async Task<IActionResult> PutSummary(List< Ref_Summary> _Summary)
        {
          await  _summary.PutSummaryAsync(_Summary);
            return Ok();
        }
        [HttpGet]
        [Route("kpisummary")]
        public async Task<IActionResult> GetSummaryForForm()
        {
            var result = await _summary.GetSummaryForFormAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("pdf")]
        public async Task<IActionResult> GetPDF([FromBody] PdfRequestVM pdfRequest)
        {
            var result = await _summary.GetPDF(pdfRequest.hospitals ,pdfRequest.pillars);
            if (result.Count == 0)
            {
               
                return Ok(0) ;
            }
            return Ok(result);
        }

    }
}
