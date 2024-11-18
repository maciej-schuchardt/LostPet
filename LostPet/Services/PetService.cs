using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace LostPet.Services
{
    public class PetService(ApplicationDbContext context)
    {
        public async Task Found(int id)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                context.Pets.Single(r => r.PetID == id).Status = Status.Found;
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Remove(int id)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                await context.Pets.Where(r => r.PetID == id).ExecuteDeleteAsync();
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Pet>> GetRangeOfPets(int position)
        {
            var pets = await context.Pets.OrderByDescending(p => p.PetID).Skip(position).Take(1).ToListAsync();
            return pets;
        }
        public async Task<List<Pet>> GetAllPets()
        {
            var pets = await context.Pets.OrderByDescending(p => p.PetID).ToListAsync();
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

        public async Task<Pet> GetPet(int id)
        {
            var pet = await context.Pets.SingleAsync(x => x.PetID == id);
            return pet;
        }
    }
}
