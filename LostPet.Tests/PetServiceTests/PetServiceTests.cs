using System.Diagnostics.CodeAnalysis;
using LostPet.Models;
using LostPet.Services;
using LostPet.Tests.Helpers;

namespace LostPet.Tests.PetServiceTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PetServiceTests : PetServiceTemplate
    {
        #region AddAsync Tests

        [Test]
        public async Task AddAsync_ShouldAddNewRecord()
        {
            // Arrange
            Pet pet = new()
            {
                Name = "TestPet",
                Species = "TestSpecies",
                Breed = "TestBreed",
                Color = "TestColor",
                Age = new Random().Next(0, 40),
                Weight = new Random().Next(2, 60),
                MicrochipID = new Random().Next(1000000, 9999999).ToString(),
                Photo = new Guid().ToString(),
                Status = (Status)new Random().Next(0, 2),
                Sex = (Sex)new Random().Next(0, 2),
                UserID = new Guid().ToString(),
                Description = "TestDescription",
                LastSeenLocation = "TestLocation",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            var recordsBeforeAdd = context.Pets.Count();

            // Act
            await Service.AddAsync(pet);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Pets.Count(), Is.GreaterThan(recordsBeforeAdd));
                Assert.That(context.Pets.Any(p => p == pet), Is.True);
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
        public async Task GetByIdAsync_ShouldReturnRequestedRecord()
        {
            TestContext.Out.WriteLine($"testname: {TestContext.CurrentContext.Test.Name} | randomId: {randomId}");
            // Act
            Pet result = await this.Service.GetByIdAsync(randomId);

            // Assert
            Assert.That(result, Is.EqualTo(context.Pets.Single(p => p.PetID == randomId)));
        }

        #region RemoveByIdAsync Tests

        [Test]
        public async Task RemoveByIdAsync_ShouldRemoveRecord()
        {
            // Arrange
            TestContext.Out.WriteLine($"randomId: {randomId}");
            int countBeforeRemove = context.Pets.Count();

            // Act
            await this.Service.RemoveByIdAsync(randomId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(context.Pets.Count(), Is.LessThan(countBeforeRemove));
                Assert.That(delegate { context.Pets.Single(p => p.PetID == randomId); }, Throws.InvalidOperationException);
            });
        }

        [Test]
        public void RemoveByIdAsync_ShouldThrowOnInvalidId()
        {
            Assert.That(async delegate { await this.Service.RemoveByIdAsync(invalidRandomId); }, Throws.InvalidOperationException);
        }

        #endregion

        #region SetFoundByIdAsync Tests

        [Test]
        public async Task SetFoundByIdAsync_ShouldSetPetAsFound()
        {
            // Arrange
            var pet = (await this.Service.GetByIdAsync(randomId)).Clone() as Pet;

            // Act
            await this.Service.SetFoundByIdAsync(randomId);
            var petAfterChange = await this.Service.GetByIdAsync(randomId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(pet.Status, Is.Not.EqualTo(Status.Found));
                Assert.That(petAfterChange.Status, Is.Not.EqualTo(pet.Status));
                Assert.That(petAfterChange.Status, Is.EqualTo(Status.Found));
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