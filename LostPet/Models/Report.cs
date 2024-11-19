using Newtonsoft.Json;
using NuGet.Protocol;

namespace LostPet.Models
{
    public class Report
    {
        public int ReportID { get; set; }
        public int PetID { get; set; }
        public string UserID { get; set; }
        public int ReportType { get; set; }
        public DateTime ReportDate { get; set; }
        public string? Details { get; set; }

        public object? Clone()
        {
            return JsonConvert.DeserializeObject<Report>(this.ToJson());
        }
    }
}
