using System.ComponentModel.DataAnnotations;

namespace LostPet.Models
{
    public class Sighting
    {
        public int SightingID { get; set; }
        public int PetID { get; set; }
        public string? Location { get; set; }
        public string UserID { get; set; }
        public DateTime SightingDate { get; set; }
        public string? Notes { get; set; }
    }
}
