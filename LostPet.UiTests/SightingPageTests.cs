using Microsoft.Playwright;

namespace LostPet.UiTests
{
    public class SightingPageTests : Template
    {
        [Test]
        public async Task SightingPage_CanAddComment()
        {
            var petName = await AddNewPost();
            var before = await page.Locator(".comment").CountAsync();
            var btns = page.GetByRole(AriaRole.Button, new() { NameString = "Dodaj komentarz" });

            await btns.First.ClickAsync();

            await page.Locator("input[name=\"Input\\.Location\"]").FillAsync(Guid.NewGuid().ToString());
            await page.Locator("input[name=\"Input\\.Notes\"]").FillAsync(Guid.NewGuid().ToString());
            await page.GetByRole(AriaRole.Button, new() { NameString = "Dodaj" }).ClickAsync();

            var after = await RetryAsync(async () =>
            {
                var temp = await page.Locator(".comment").CountAsync();
                return temp > before ? temp : throw new Exception("");
            });

            Assert.That(after, Is.GreaterThan(before));
        }

        [Test]
        public async Task SightingPage_CanRemoveComment()
        {
            await SightingPage_CanAddComment();

            var before = await page.Locator(".comment").CountAsync();
            var btns = page.GetByRole(AriaRole.Button, new() { NameString = "Usuń komentarz" });
            await btns.First.ClickAsync();

            var after = await RetryAsync(async () =>
            {
                var temp = await page.Locator(".comment").CountAsync();
                return temp < before ? temp : throw new Exception("");
            });

            Assert.That(after, Is.LessThan(before));
        }
    }
}
