namespace DataHub_System_Goal.Models
{
    public partial class RefHospital
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int? DivisionId { get; set; }

        public int? OrganizationId { get; set; }
        
    }


    public partial class RefOperatingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Boolean? isActive { get; set; }

    }
}
