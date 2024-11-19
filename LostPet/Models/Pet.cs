using Newtonsoft.Json;
using NuGet.Protocol;

namespace LostPet.Models
{
    public enum Status
    {
        Lost,
        Stray,
        Found,
    }
    public class Pet : ICloneable
    {
        public int PetID { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string? Breed { get; set; }
        public string? Color { get; set; }
        public int? Age { get; set; }
        public double? Weight { get; set; }
        public string? MicrochipID { get; set; }
        public string Photo { get; set; }
        public Status Status { get; set; }
        public string? LastSeenLocation { get; set; }
        public string? Description { get; set; }
        public string UserID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public object? Clone()
        {
            return JsonConvert.DeserializeObject<Pet>(this.ToJson());
        }
    }
}
