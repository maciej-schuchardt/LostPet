using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace LostPet.Services
{
    public class PetService(ApplicationDbContext context) : IService
    {
        public async Task SetFoundByIdAsync(int id)
        {
            try
            {
                context.Pets.Single(r => r.PetID == id).Status = Status.Found;
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveByIdAsync(int id)
        {
            try
            {
                context.Remove(await context.Pets.SingleAsync(r => r.PetID == id));
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Pet> AddAsync(Pet pet)
        {
            EntityEntry<Pet> entityEntry = null;
            try
            {
                entityEntry = await context.AddAsync(pet);
                await context.SaveChangesAsync();

                return entityEntry.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Pet> GetByIdAsync(int id)
        {
            var pet = await context.Pets.SingleAsync(x => x.PetID == id);
            return pet;
        }
    }
}
