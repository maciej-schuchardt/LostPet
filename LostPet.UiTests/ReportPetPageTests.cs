using Microsoft.Playwright;

namespace LostPet.UiTests
{
    public class ReportPetPageTests : Template
    {
        [Test]
        public async Task ReportPetPage_ValidationMessagesAreVisible()
        {
            await Login(new("test@mail.com", "Admin1!"), page, goToLogin: true);
            //await page.GotoAsync("http://localhost:5000/report", new() { Timeout = 30 * 1000 });
            await page.Locator("#reports").ClickAsync();
            await page.WaitForURLAsync("http://localhost:5000/report");
            await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zgłoś" }).ClickAsync();
            await Task.Delay(2000);
            var speciesCount = await page.GetByText(new Regex("Pole\\s+\"Gatunek\"\\s+jest\\s+wymagane")).CountAsync();
            var colorCount = await page.GetByText(new Regex("Pole\\s+\"Kolor\"\\s+jest\\s+wymagane")).CountAsync();
            var statusCount = await page.GetByText(new Regex("Pole\\s+\"Status\"\\s+jest\\s+wymagane")).CountAsync();
            var lastSeenCount = await page.GetByText(new Regex("Pole\\s+\"Ostatnio\\s+widziany\"\\s+jest\\s+wymagane")).CountAsync();
            var descriptionCount = await page.GetByText(new Regex("Pole\\s+\"Opis\"\\s+jest\\s+wymagane")).CountAsync();

            Assert.That(speciesCount, Is.EqualTo(1));
            Assert.That(colorCount, Is.EqualTo(1));
            Assert.That(statusCount, Is.EqualTo(1));
            Assert.That(lastSeenCount, Is.EqualTo(1));
            Assert.That(descriptionCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ReportPetPage_RedirectsWhenNotLoggedIn()
        {
            var logoutButton = await page.Locator("#logout").CountAsync();
            if (logoutButton > 0)
            {
                await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("^Wyloguj .*$") }).ClickAsync();
                await page.WaitForURLAsync("http://localhost:5000/");
            }
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zgłoś!" }).ClickAsync();
            await page.WaitForURLAsync("http://localhost:5000/Account/Login?ReturnUrl=%2Freport");
            var loginButton = await page.Locator("#loginSubmit").CountAsync();

            Assert.That(loginButton, Is.EqualTo(1));
        }

        [Test]
        public async Task ReportPageTest_CanAddNewPost()
        {
            await AddNewPost();
        }
    }
}
