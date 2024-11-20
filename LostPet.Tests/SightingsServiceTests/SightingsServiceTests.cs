using System.Diagnostics.CodeAnalysis;
using LostPet.Models;
using LostPet.Tests.Helpers;

namespace LostPet.Tests.SightingsServiceTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SightingsServiceTests : SightingsServiceTestsTemplate
    {
        [Test]
        public async Task GetAllAsync_ShouldReturnAllSightings() {
            // Arrange
            var expectedSightingsCount = context.Sightings.Count();

            // Act
            var all = await Service.GetAllAsync();

            // Assert
            Assert.That(all.Count, Is.EqualTo(expectedSightingsCount));
        }

        #region AddAsync Tests

        [Test]
        public async Task AddAsync_ShouldAddNewRecord()
        {
            // Arrange
            Sighting sighting = new()
            {
                PetID = new Random().Next(1, 6),
                Location = Guid.NewGuid().ToString(),
                UserID = Guid.NewGuid().ToString(),
                SightingDate = DateTime.Now,
                Notes = Guid.NewGuid().ToString()
            };

            var recordsBeforeAdd = context.Sightings.Count();

            // Act
            await Service.AddAsync(sighting);

            // Assert
            Assert.That(context.Sightings.Count(), Is.GreaterThan(recordsBeforeAdd));
        }
    
        [Test]
        public void AddAsync_ShouldThrowExceptionOnInvalidObject()
        {
            // Act Assert
            Assert.That(async delegate { await this.Service.AddAsync(new()); }, ThrowsHelper.DbUpdateException);
        }

        #endregion
    }
}