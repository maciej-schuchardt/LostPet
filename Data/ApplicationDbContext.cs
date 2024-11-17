using LostPet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LostPet.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Sighting> Sightings { get; set; }
    }
}
