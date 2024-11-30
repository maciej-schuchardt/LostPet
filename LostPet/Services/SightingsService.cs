using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace LostPet.Services
{
    public class SightingsService(ApplicationDbContext context) : IService
    {
        public async Task<List<SightingsViewModel>> GetAllAsync()
        {
            var data = await context.Sightings.Join(
                context.Pets,
                s => s.PetID,
                p => p.PetID,
                (s, p) =>
                new SightingsViewModel
                {
                    Sighting = s,
                    Pet = p,
                    ReporterEmail = context.Users.Single(u => u.Id == s.UserID).Email,
                }).OrderByDescending(x => x.Pet.PetID).ToListAsync();

            return data;
        }

        public async Task<Sighting> AddAsync(Sighting sighting)
        {
            EntityEntry<Sighting> entityEntry = null;
            try
            {
                entityEntry = await context.AddAsync(sighting);
                await context.SaveChangesAsync();

                return entityEntry.Entity;
            }
            catch
            {
                throw;
            }
        }

        public async Task RemoveByIdAsync(int id)
        {
            try
            {
                context.Remove(await context.Sightings.SingleAsync(s => s.SightingID == id));
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
