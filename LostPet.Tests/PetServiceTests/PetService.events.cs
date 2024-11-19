using LostPet.Data;
using LostPet.Models;
using LostPet.Services;
using Microsoft.EntityFrameworkCore;

namespace LostPet.Tests.PetServiceTests;

public partial class PetServiceTests
{
       private ApplicationDbContext context;
        private PetService petService;

        private int randomId;
        private int invalidRandomId;

        [SetUp]
        public async Task BeforeEveryTest()
        {
            if (TestContext.CurrentContext.Test.Name != "AddAsync_ShouldAddNewRecord" || TestContext.CurrentContext.Test.Name != "AddAsync_ShouldThrowExceptionOnInvalidObject")
            {
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
                        Status = (Status)new Random().Next(0, 1),
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
                        Status = (Status)new Random().Next(0, 1),
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
                        Status = (Status)new Random().Next(0, 1),
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
                        Status = (Status)new Random().Next(0, 1),
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
                        Status = (Status)new Random().Next(0, 1),
                        UserID = new Guid().ToString(),
                        Description = "TestDescription",
                        LastSeenLocation = "TestLocation",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                );
                await context.SaveChangesAsync();
                await context.Database.EnsureCreatedAsync();
                randomId = new Random().Next(1, context.Pets.Count());
                invalidRandomId = new Random().Next(context.Pets.Count() + 1, context.Pets.Count() * 4);
            }
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

        [OneTimeSetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new ApplicationDbContext(optionsBuilder.Options);
            petService = new PetService(context);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            context.Dispose();
        }
}
