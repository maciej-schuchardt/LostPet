using Microsoft.Playwright;

namespace LostPet.UiTests
{
    [TestFixture]
    public class HomePageTests : Template
    {
        [Test]
        [TestCase("reports")]
        [TestCase("login")]
        [TestCase("register")]
        public async Task HomePage_NotAuthorized_MenuButtonIsVisible(string buttonId)
        {
            await Logout();
            var button = await page.Locator($"#{buttonId}").CountAsync();
            Assert.That(button, Is.EqualTo(1));
        }

        [Test]
        public async Task HomePage_LogoIsVisible()
        {
            var logo = await page.Locator("#logo").CountAsync();
            Assert.That(logo, Is.EqualTo(1));
        }

        [Test]
        public async Task HomePage_FilterBarIsVisible()
        {
            var filtersBar = await page.Locator("#filters").CountAsync();
            Assert.That(filtersBar, Is.GreaterThan(0));
        }

        [Test]
        public async Task HomePage_PostsAreLoaded()
        {
            await page.Locator("#loading").WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await page.Locator("#loading").WaitForAsync(new() { State = WaitForSelectorState.Detached });

            var posts = await page.Locator(".card").CountAsync();
            Assert.That(posts, Is.GreaterThan(0));
        }

        [Test]
        [TestCase("reports")]
        [TestCase("myreports")]
        [TestCase("logout")]
        public async Task HomePage_Authorized_MenuButtonIsVisible(string buttonId)
        {
            // Arrange
            await Login(new("test@mail.com", "Admin1!"), page, goToLogin: true);

            // Act
            var button = await page.Locator($"#{buttonId}").CountAsync();

            // Assert
            Assert.That(button, Is.EqualTo(1));
        }
    }
}
