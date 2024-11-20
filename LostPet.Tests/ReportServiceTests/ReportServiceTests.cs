using System.Diagnostics.CodeAnalysis;
using LostPet.Models;
using LostPet.Tests.Helpers;

namespace LostPet.Tests.ReportServiceTests
{
    [ExcludeFromCodeCoverage]
    public class ReportServiceTests : ReportServiceTemplate
    {
        #region AddAsync Tests

        [Test]
        public async Task AddAsync_ShouldAddNewRecord()
        {
            // Arrange
            int petId = 999;

            Pet pet = new()
            {
                PetID = petId,
                Name = "TestPet",
                Species = "TestSpecies",
                Breed = "TestBreed",
                Color = "TestColor",
                Age = new Random().Next(0, 40),
                Weight = new Random().Next(2, 60),
                MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                Photo = new Guid().ToString(),
                Status = (Status)new Random().Next(0, 2),
                UserID = new Guid().ToString(),
                Description = "TestDescription",
                LastSeenLocation = "TestLocation",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await context.AddAsync(pet);
            await context.SaveChangesAsync();

            Report report = new()
            {
              PetID = petId,
              UserID = context.Pets.First().UserID,
              ReportType = (int)context.Pets.First().Status,
              ReportDate = (DateTime)context.Pets.First().CreatedAt,
              Details = context.Pets.First().Description
            };
            var recordsBeforeAdd = context.Reports.Count();

            // Act
            await Service.AddAsync(report);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Reports.Count(), Is.GreaterThan(recordsBeforeAdd));
                Assert.That(context.Reports.Any(p => p.PetID == pet.PetID), Is.True);
            });
        }
    
        [Test]
        public void AddAsync_ShouldThrowExceptionOnInvalidObject()
        {
            // Act Assert
            Assert.That(async delegate { await this.Service.AddAsync(new()); }, ThrowsHelper.DbUpdateException);
        }

        #endregion

        [Test]
        public async Task GetByFilterAsync_ShouldReturnRequestedRecords()
        {
            // Act
            var reports = await Service.GetByFilterAsync(x => x.Status == (Status) new Random().Next(2));

            // Assert
            Assert.That(reports, Is.Not.Empty);
        }

        #region SetFoundByIdAsync Tests

        [Test]
        public async Task SetFoundByIdAsync_ShouldSetPetAsFound()
        {
            // Arrange
            var report = (await this.Service.GetByFilterAsync(x => x.PetID == randomId)).First().Report.Clone() as Report;

            // Act
            await this.Service.SetFoundByIdAsync(randomId);
            var reportAfterChange = (await this.Service.GetByFilterAsync(x => x.PetID == randomId)).First().Report;
            // var reportAfterChange = await this.Service.GetByIdAsync(randomId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(report.ReportType, Is.Not.EqualTo((int)Status.Found));
                Assert.That(reportAfterChange.ReportType, Is.Not.EqualTo(report.ReportType));
                Assert.That(reportAfterChange.ReportType, Is.EqualTo((int)Status.Found));
            });
        }

        [Test]
        public void SetFoundByIdAsync_ShouldThrowOnInvalidId()
        {
            Assert.That(async delegate { await this.Service.SetFoundByIdAsync(invalidRandomId); }, Throws.InvalidOperationException);
        }

        #endregion
    }
}