using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace LostPet.Services
{
    public class SightingsService(ApplicationDbContext context)
    {
        public async Task<List<SightingsViewModel>> GetAll()
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

        public async Task<Sighting> AddNewSighting(Sighting sighting)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            EntityEntry<Sighting> entityEntry = null;
            try
            {
                entityEntry = await context.AddAsync(sighting);
                await context.SaveChangesAsync();
                transaction.Commit();

                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
