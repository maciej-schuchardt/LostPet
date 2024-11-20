using System.Diagnostics.CodeAnalysis;
using LostPet.Data;
using LostPet.Models;
using LostPet.Services;
using Microsoft.EntityFrameworkCore;

namespace LostPet.Tests.Templates;

[ExcludeFromCodeCoverage]
public abstract class CommonEvents<T> where T : IService
{
    protected int randomId;

    protected int invalidRandomId;

    public required ApplicationDbContext context;

    protected abstract T Service { get; }

    [OneTimeSetUp]
    public virtual void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        context = new ApplicationDbContext(optionsBuilder.Options);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [TearDown]
    public async Task AfterEach()
    {
        context.ChangeTracker
            .Entries()
            .ToList()
            .ForEach(e => e.State = EntityState.Detached);
        await context.Database.EnsureDeletedAsync();
    }

    
    [SetUp]
    public async Task BeforeEveryTest()
    {
        for (int i = 1; i <= 30; i++) {
            await context.Users.AddAsync(new ApplicationUser() {
                Id = i.ToString(),
                Email = Guid.NewGuid().ToString() + "@mail.com"
            });
        }
        await context.SaveChangesAsync();

        for (int i = 1; i <= 5; i++) {
            await context.Pets.AddAsync(
                new Pet()
                {
                    Name = "TestPet" + i,
                    Species = "TestSpecies" + i,
                    Breed = "TestBreed" + i,
                    Color = "TestColor" + i,
                    Age = new Random().Next(0, 40),
                    Weight = new Random().Next(2, 60),
                    MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                    Photo = Guid.NewGuid().ToString(),
                    Status = (Status) (5 % i),
                    UserID = Guid.NewGuid().ToString(),
                    Description = "TestDescription" + i,
                    LastSeenLocation = "TestLocation" + i,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );
        }
        await context.SaveChangesAsync();

        for (int i = 1; i <= 5; i++) {
            var pet = context.Pets.Single(p => p.PetID == i);
            await context.Reports.AddAsync(new Report(){
                PetID = pet.PetID,
                UserID = new Random().Next(1, 31).ToString(),
                ReportType = (int)pet.Status,
                Details = pet.Description
            });
            for (int j = 1; j <= 5; j++) {
                await context.Sightings.AddAsync(new Sighting() {
                    PetID = pet.PetID,
                    Location = Guid.NewGuid().ToString(),
                    UserID = new Random().Next(1, 31).ToString(),
                    SightingDate = DateTime.Now,
                    Notes = Guid.NewGuid().ToString()
                });
            }
        }
        await context.SaveChangesAsync();

        // await context.Sightings.Add();
        await context.Database.EnsureCreatedAsync();
        randomId = new Random().Next(1, context.Pets.Count());
        invalidRandomId = new Random().Next(context.Pets.Count() + 1, context.Pets.Count() * 4);
    }
}
