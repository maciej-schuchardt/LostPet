using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostPet.UiTests
{
    public abstract class Template
    {
        public record User(string Email, string Password);
        public IPage page;
        IPlaywright playwright;
        IBrowser browser;
        IBrowserContext context;
        Process sut;
        public User testUser = new("admin@mail.com", "Admin1!");


        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            try
            {
                string sutPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
                sut = StartSystemUnderTest($@"{sutPath}\LostPet\", "dotnet", "run -- uiTests");

                playwright = await Playwright.CreateAsync();
                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                    SlowMo = 400,
                    //Args = ["--start-maximized"]
                });
                context = await browser.NewContextAsync(new()
                {
                    //ViewportSize = ViewportSize.NoViewport
                });
                page = await context.NewPageAsync();
                await RetryAsync(async () =>
                {
                    await page.GotoAsync("http://localhost:5000/", new() { Timeout = 30 * 1000 });
                });
                //int retries = 5;
                //while (retries > 0)
                //{
                //    try
                //    {
                //        await page.GotoAsync("http://localhost:5000/", new() { Timeout = 30 * 1000 });
                //        break;
                //    }
                //    catch (Exception e)
                //    {
                //        await Task.Delay(5000);
                //        retries--;
                //        if (retries <= 0) { throw; }
                //    }
                //}
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            }
            catch(Exception ex)
            {

            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await page.CloseAsync();
            await context.CloseAsync();
            await browser.CloseAsync();
            await context.DisposeAsync();
            await browser.DisposeAsync();
            playwright.Dispose();
            sut.Kill(true);
            sut.Dispose();
        }

        public async Task RetryAsync(Func<Task> func)
        {
            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    await func();
                    break;
                }
                catch (Exception e)
                {
                    await Task.Delay(5000);
                    retries--;
                    if (retries <= 0) { throw; }
                }

            }
        }

        public async Task<T> RetryAsync<T>(Func<Task<T>> func)
        {
            int retries = 5;
            T result = default;
            while (retries > 0)
            {
                try
                {
                    result = await func();
                    break;
                    //await page.GotoAsync("http://localhost:5000/", new() { Timeout = 30 * 1000 });
                    //break;
                }
                catch (Exception e)
                {
                    await Task.Delay(5000);
                    retries--;
                    if (retries <= 0) { throw; }
                }

            }
            return result;
        }

        public async Task<bool> Login(User user, IPage page, bool goToLogin)
        {
            var logoutButton = await page.Locator("#logout").CountAsync();
            if (logoutButton > 0)
            {
                var logout = page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex($"^Wyloguj {user.Email}$") });
                if (await logout.CountAsync() > 0) { return  true; }
                await logout.ClickAsync();
                Task.Delay(3000);
            }
            if (goToLogin) { await page.GotoAsync("http://localhost:5000/Account/Login", new() { Timeout = 30 * 1000 }); }
            await page.GetByPlaceholder("name@example.com").WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await page.GetByPlaceholder("name@example.com").ClickAsync();
            await page.GetByPlaceholder("name@example.com").FillAsync(user.Email);
            await page.GetByPlaceholder("name@example.com").PressAsync("Tab");
            await page.GetByPlaceholder("password").FillAsync(user.Password);
            await page.GetByPlaceholder("password").PressAsync("Enter");
            await page.WaitForURLAsync("http://localhost:5000/");
            logoutButton = await page.Locator("#logout").CountAsync();
            //Assert.That(logoutButton, Is.EqualTo(1));
            return logoutButton > 0;

        }

        public async Task Logout()
        {
            var logoutButton = page.Locator("#logout");
            if (await logoutButton.CountAsync() > 0)
            {
                await logoutButton.ClickAsync();
                await page.WaitForURLAsync("http://localhost:5000/");
            }
        }

        public async Task<string> AddNewPost()
        {
            // Arrange
            await Login(new("test@mail.com", "Admin1!"), page, goToLogin: true);
            var petName = Guid.NewGuid().ToString();
            await page.Locator("#reports").ClickAsync();
            await Task.Delay(2000);

            // Act
            await page.Locator("input[name=\"Input\\.Name\"]").FillAsync(petName);
            await page.Locator("input[name=\"Input\\.Species\"]").FillAsync("Pies");
            await page.Locator("input[name=\"Input\\.Breed\"]").FillAsync("Jamnik");
            await page.Locator("input[name=\"Input\\.Color\"]").FillAsync("Brązowy");
            await page.Locator("input[name=\"Input\\.Age\"]").FillAsync("2");
            await page.Locator("input[name=\"Input\\.Weight\"]").FillAsync("5");
            await page.Locator("input[name=\"Input\\.MicrochipID\"]").FillAsync(new Random().Next(1000000, 9999999).ToString());
            await page.Locator("input[name=\"Input\\.LastSeenLocation\"]").FillAsync("Gdańsk Morena");
            await page.Locator("input[name=\"Input\\.Description\"]").FillAsync("Fajny piesek");
            await page.Locator("input[name=\"Input\\.Attachment\"]").SetInputFilesAsync(["TestResources\\Jamnik.jpg"]);
            await page.Locator("select[name=\"Input\\.Status\"]").SelectOptionAsync(["Lost"]);
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zgłoś" }).ClickAsync();
            await page.WaitForURLAsync("http://localhost:5000/");
            //await Task.Delay(5000);

            //await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("^Wyloguj .*$") }).ClickAsync();
            var newPost = await RetryAsync(async () =>
            {
                var post = page.GetByRole(AriaRole.Heading, new() { NameRegex = new Regex($"^.* {petName}$") });
                var count = await post.CountAsync();
                return count == 0 ? throw new Exception() : count;
            });
            Assert.That(newPost, Is.EqualTo(1));
            return petName;
        }

        private Process StartProcess(string workingDir, string processName, string args)
        {
            Process process = new();
            process.StartInfo.WorkingDirectory = workingDir;
            process.StartInfo.FileName = processName;
            process.StartInfo.Arguments = args;
            process.Start();
            return process;
        }

        private Process StartSystemUnderTest(string workingDir, string processName, string args = "")
        {
            var process = StartProcess(workingDir, processName, args);
            Task.Delay(30000);
            return process;
        }
    }
}
