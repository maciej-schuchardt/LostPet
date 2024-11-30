using Microsoft.Playwright;

namespace LostPet.UiTests
{
    public class RegisterPageTests : Template
    {
        [Test]
        public async Task RegisterPage_CanLoginUsingLoginButton()
        {
            //await page.GotoAsync("http://localhost:5000/");
            await page.GetByRole(AriaRole.Button, new() { NameString = "Utwórz nowe konto" }).ClickAsync();
            await page.WaitForURLAsync("http://localhost:5000/Account/Register");
            await page.GetByPlaceholder("name@example.com").ClickAsync();
            await page.GetByPlaceholder("name@example.com").FillAsync($"test_{Guid.NewGuid()}@mail.com");
            await page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await page.Locator("input[name=\"Input\\.Password\"]").FillAsync("Admin1!");
            await page.Locator("input[name=\"Input\\.Password\"]").PressAsync("Tab");
            await page.Locator("input[name=\"Input\\.ConfirmPassword\"]").FillAsync("Admin1!");
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zarejestruj się" }).ClickAsync();
            await page.WaitForURLAsync("http://localhost:5000/");
            var logoutButton = await page.Locator("#logout").CountAsync();

            Assert.That(logoutButton, Is.GreaterThan(0));
        }
    }
}
