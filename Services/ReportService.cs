using LostPet.Data;
using LostPet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Composition;
using System.Linq;

namespace LostPet.Services
{
    public class ReportService(ApplicationDbContext context)
    {
        public async Task Found(int id)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                context.Reports.Single(r => r.PetID == id).ReportType = (int)Status.Found;
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<HomeViewModel>> GetSpecificReports(Func<Pet, bool> filter)
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
                    //p.PetID,
                }).OrderByDescending(x => x.Report.ReportID).ToListAsync();

            return reports.Where(x => filter(x.Pet)).ToList();
        }

        public async Task<List<HomeViewModel>> GetAllReports()
        {
            //var pets = await context.Pets.ToListAsync();
            var reports = await context.Reports.Join(
                context.Pets,
                r => r.PetID,
                p => p.PetID,
                (r, p) =>
                new HomeViewModel
                {
                    Report = r,
                    Pet = p
                    //p.PetID,
                }).OrderByDescending(x => x.Report.ReportID).ToListAsync();

            return reports;
        }

        public async Task<Report> AddNewReport(Report report)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
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

        public async Task Remove(int id)
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                await context.Reports.Where(r => r.ReportID == id).ExecuteDeleteAsync();
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
