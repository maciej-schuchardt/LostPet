using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LostPet.Services
{
    public class ReportService(ApplicationDbContext context) : IService
    {
        public async Task SetFoundByIdAsync(int id)
        {
            try
            {
                context.Reports.Single(r => r.PetID == id).ReportType = (int)Status.Found;
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<HomeViewModel>> GetByFilterAsync(Func<Pet, bool> filter)
        {
            var reports = await context.Reports.Join(
                context.Pets,
                r => r.PetID,
                p => p.PetID,
                (r, p) =>
                new HomeViewModel
                {
                    Report = r,
                    Pet = p
                }).OrderByDescending(x => x.Report.ReportID).ToListAsync();

            return reports.Where(x => filter(x.Pet)).ToList();
        }

        public async Task<Report> AddAsync(Report report)
        {
            EntityEntry<Report> entityEntry = null;
            try
            {
                entityEntry = await context.AddAsync(report);
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
                context.Remove(await context.Reports.SingleAsync(s => s.ReportID == id));
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateDetailsByIdAsync(int id, string details)
        {
            try
            {
                var entity = await context.Reports.SingleAsync(s => s.ReportID == id);
                entity.Details = details;
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
