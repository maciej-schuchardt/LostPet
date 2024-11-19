using System.Diagnostics.CodeAnalysis;
using LostPet.Data;
using LostPet.Models;
using LostPet.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        // if (TestContext.CurrentContext.Test.Name != "AddAsync_ShouldAddNewRecord" || TestContext.CurrentContext.Test.Name != "AddAsync_ShouldThrowExceptionOnInvalidObject")
        // {
            await context.Pets.AddRangeAsync(
                new Pet()
                {
                    Name = "TestPet",
                    Species = "TestSpecies",
                    Breed = "TestBreed",
                    Color = "TestColor",
                    Age = new Random().Next(0, 40),
                    Weight = new Random().Next(2, 60),
                    MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                    Photo = new Guid().ToString(),
                    Status = (Status)0,
                    UserID = new Guid().ToString(),
                    Description = "TestDescription",
                    LastSeenLocation = "TestLocation",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Pet()
                {
                    Name = "TestPet1",
                    Species = "TestSpecies1",
                    Breed = "TestBreed1",
                    Color = "TestColor1",
                    Age = new Random().Next(0, 40),
                    Weight = new Random().Next(2, 60),
                    MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                    Photo = new Guid().ToString(),
                    Status = (Status)0,
                    UserID = new Guid().ToString(),
                    Description = "TestDescription",
                    LastSeenLocation = "TestLocation",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Pet()
                {
                    Name = "TestPet2",
                    Species = "TestSpecies2",
                    Breed = "TestBreed2",
                    Color = "TestColor2",
                    Age = new Random().Next(0, 40),
                    Weight = new Random().Next(2, 60),
                    MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                    Photo = new Guid().ToString(),
                    Status = (Status)1,
                    UserID = new Guid().ToString(),
                    Description = "TestDescription",
                    LastSeenLocation = "TestLocation",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Pet()
                {
                    Name = "TestPet3",
                    Species = "TestSpecies3",
                    Breed = "TestBreed3",
                    Color = "TestColor3",
                    Age = new Random().Next(0, 40),
                    Weight = new Random().Next(2, 60),
                    MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                    Photo = new Guid().ToString(),
                    Status = (Status)1,
                    UserID = new Guid().ToString(),
                    Description = "TestDescription",
                    LastSeenLocation = "TestLocation",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new Pet()
                {
                    Name = "TestPet4",
                    Species = "TestSpecies4",
                    Breed = "TestBreed4",
                    Color = "TestColor4",
                    Age = new Random().Next(0, 40),
                    Weight = new Random().Next(2, 60),
                    MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                    Photo = new Guid().ToString(),
                    Status = (Status)1,
                    UserID = new Guid().ToString(),
                    Description = "TestDescription",
                    LastSeenLocation = "TestLocation",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );
            await context.SaveChangesAsync();
            for (int i = 1; i <= context.Pets.Count(); i++) {
                var pet = context.Pets.Single(p => p.PetID == i);
                await context.Reports.AddAsync(new Report(){
                    PetID = pet.PetID,
                    UserID = pet.UserID,
                    ReportType = (int)pet.Status,
                    Details = pet.Description
                });
            }
            await context.SaveChangesAsync();
            await context.Database.EnsureCreatedAsync();
            randomId = new Random().Next(1, context.Pets.Count());
            invalidRandomId = new Random().Next(context.Pets.Count() + 1, context.Pets.Count() * 4);
        // }
    }
}
