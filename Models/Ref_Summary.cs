using System;

namespace DataHub_System_Goal.Models
{
    public class Ref_Summary
    {
        public int OperatingModelId { get; set; }
        public int HospitalId { get; set; }
        public int GoalId { get; set; }
        public int SubGoalId { get; set; }
        public int KPIId { get; set; }
        public string? ReportType { get; set; }
        public string? MTD_ACTUAL { get; set; }
        public string? MTD_BUDGET { get; set; }
        public string? MTD_PRIOR_YEAR { get; set; }
        public string? MTD_BUDGET_VARIANCE { get; set; }
        public string? MTD_PRIOR_YEAR_VARIANCE { get; set; }
        public string? YTD_ACTUAL { get; set; }
        public string? YTD_BUDGET { get; set; }
        public string? YTD_PRIOR_YEAR { get; set; }
        public string? YTD_BUDGET_VARIANCE { get; set; }
        public string? YTD_PRIOR_YEAR_VARIANCE { get; set; }
        public string? URL { get; set; }
        public string? Comment { get; set; }
        public string? ValueType { get; set; }
        public string? YTD_PRIOR_YEAR_VAR_SIGN { get; set; }
        public string? MTD_PRIOR_YEAR_VAR_SIGN { get; set; }

        public string? YTD_BUDGET_VAR_SIGN { get; set; }
        public DateTime? REC_MODIFY_DT { get; set; }
        public string? REC_MODIFY_BY {  get; set; }

        public string? MTD_BUDGET_VAR_SIGN { get; set; }
        public string? KPI_DEFINATION { get; set; }
        public string? ENTITY_LEVEL { get; set; }
        public Boolean IsAvaliable { get; set; }


        public string? MTD_BUDGET_VAR_CLR { get; set; }
        public string? MTD_PRIOR_YEAR_VAR_CLR { get; set; }
        public string? YTD_BUDGET_VAR_CLR { get; set; }
        public string? YTD_PRIOR_YEAR_VAR_CLR { get; set; }
    }



    public class Ref_PDF
    {
        public int OPERATING_MODEL_ID { get; set; }
        public string OPERATING_MODEL { get; set; }
        public string PILLAR_NAME { get; set; }
        public string SUB_GOAL_TITLE { get; set; }
        public int PILLARS_ID { get; set; }
        public int SUB_GOAL_ID { get; set; }
        public int KPI_ID { get; set; }
        public string ENTITY_LEVEL { get; set; }
        public string KPI_TITLE { get; set; }
        public int HOSPITAL_ID { get; set; }
        public string HOSPITAL_NAME { get; set; }
        public string? MONTH_QUARTER { get; set; }
        public string? MTD_QTD_ACTUAL { get; set; }
        public string? MTD_BUDGET_TARGET { get; set; }
        public string? MTD_PRIOR_YEAR { get; set; }
        public string? MTD_BUDGET_VARIANCE { get; set; }
        public string? MTD_PRIOR_YEAR_VARIANCE { get; set; }
        public string? YTD_ACTUAL { get; set; }
        public string? YTD_BUDGET_TARGET { get; set; }
        public string? YTD_PRIOR_YEAR { get; set; }
        public string? YTD_BUDGET_VARIANCE { get; set; }
        public string? YTD_PRIOR_YEAR_VARIANCE { get; set; }
        public string? URL { get; set; }
        public string? Comment { get; set; }
        public string? ValueType { get; set; }
        public string? YTD_PRIOR_YEAR_VAR_SIGN { get; set; }
        public string? MTD_PRIOR_YEAR_VAR_SIGN { get; set; }

        public string? YTD_BUDGET_VAR_SIGN { get; set; }

        public string? MTD_BUDGET_VAR_SIGN { get; set; }

        //public string? MTD_BUDGET_VAR_CLR { get; set; }
        //public string? MTD_PRIOR_YEAR_VAR_CLR { get; set; }
        //public string? YTD_BUDGET_VAR_CLR { get; set; }
        //public string? YTD_PRIOR_YEAR_VAR_CLR { get; set; }

    }
}
