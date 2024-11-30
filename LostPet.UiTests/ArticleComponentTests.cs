using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostPet.UiTests
{
    public class ArticleComponentTests : Template
    {
        [Test]
        public async Task Article_CanDeleteOwnPosts()
        {
            await AddNewPost();
            var buttons = page.GetByRole(AriaRole.Button, new() { NameString = "Usuń" });

            var initialButtonCount = await buttons.CountAsync();
            for (int i = initialButtonCount - 1; i >= 0; i--)
            {
                await buttons.Nth(i).ClickAsync();
                await Task.Delay(5000);
                var currentButtonCount = await page.GetByRole(AriaRole.Button, new() { NameString = "Usuń" }).CountAsync();
                Assert.That(currentButtonCount, Is.EqualTo(i));
            }
        }

        [Test]
        public async Task Article_CanSetAsFound()
        {
            await AddNewPost();
            var before = await page.GetByRole(AriaRole.Heading, new() { NameRegex = new Regex(@"^\[ODNALEZIONY\].*$") }).CountAsync();
            await page.GetByRole(AriaRole.Button, new() { NameString = "Odnaleziony" }).Nth(0).ClickAsync();
            var after = await page.GetByRole(AriaRole.Heading, new() { NameRegex = new Regex(@"^\[ODNALEZIONY\].*$") }).CountAsync();
            Assert.That(after, Is.GreaterThan(before));
        }
    }
}
