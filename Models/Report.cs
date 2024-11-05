namespace LostPet.Models
{
    public class Report
    {
        public int ReportID { get; set; }
        public required int PetID { get; set; }
        public required string UserID { get; set; }
        public required int ReportType { get; set; }
        public DateTime ReportDate { get; set; }
        public string? Details { get; set; }
    }
}
