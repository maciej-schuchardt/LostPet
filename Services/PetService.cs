using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace LostPet.Services
{
    public class PetService(ApplicationDbContext context)
    {
        public async Task<List<Pet>> GetAllPets()
        {
            var pets = await context.Pets.ToListAsync();
            return pets;
        }

        public async Task<Pet> AddNewPet(Pet pet)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                EntityEntry<Pet> entityEntry = null;
                try
                {
                    entityEntry = await context.AddAsync(pet);
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
}
