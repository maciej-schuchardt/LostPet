namespace LostPet.UiTests
{
    public class LoginPageTests : Template
    {
        [Test]
        public async Task LoginPage_CanLoginUsingLoginButton()
        {
            await page.Locator("#login").ClickAsync();
            var loggedin = await Login(new("test@mail.com", "Admin1!"), page, goToLogin: false);
            Assert.That(loggedin, Is.True);
        }
    }
}
