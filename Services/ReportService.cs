using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace LostPet.Services
{
    public class ReportService(ApplicationDbContext context)
    {
        public async Task<List<Report>> GetAllReports()
        {
            var reports = await context.Reports.ToListAsync();

            return reports;
        }

        public async Task<Report> AddNewReport(Report report)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                EntityEntry<Report> entityEntry = null;
                try
                {
                    entityEntry = await context.AddAsync(report);
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
