namespace LostPet.Models
{
    public enum Status
    {
        Lost,
        Found
    }
    public class Pet
    {
        public int PetID { get; set; }
        public required string Name { get; set; }
        public required string Species { get; set; }
        public string? Breed { get; set; }
        public string? Color { get; set; }
        public int? Age { get; set; }
        public double? Weight { get; set; }
        public string? MicrochipID { get; set; }
        public required string Photo { get; set; }
        public required Status Status { get; set; }
        public string? LastSeenLocation { get; set; }
        public string? Description { get; set; }
        public required string UserID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
