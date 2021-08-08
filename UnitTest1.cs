using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace playwright_demo
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public async Task VerifyThemeChanged()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev/dotnet");
            await page.WaitForLoadStateAsync(state: LoadState.DOMContentLoaded);
            var currentTheme = await page.GetAttributeAsync("html", "data-theme");
            await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(),
                "before-theme-change.png"), await page.ScreenshotAsync());
            await page.ClickAsync("[class=\"react-toggle-track\"]");
            var changedTheme = await page.GetAttributeAsync("html", "data-theme");
            Assert.AreNotEqual(currentTheme, changedTheme, $"Theme did not change");
            await File.WriteAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(),
                "after-theme-change.png"), await page.ScreenshotAsync());

            TestContext.AddTestAttachment(Path.Combine(Directory.GetCurrentDirectory(),
                "before-theme-change.png"), "before-theme-change.png");
            TestContext.AddTestAttachment(Path.Combine(Directory.GetCurrentDirectory(),
                "after-theme-change.png"), "after-theme-change.png");
        }
    }
}