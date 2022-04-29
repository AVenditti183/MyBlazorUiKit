using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MyBlazorUiKit.Services;
using NUnit.Framework;

namespace MyBlazorApp.Tests
{
    [TestFixture]
    public class StartFixture
    {
        [Test]
        public void StartApplication_ServiceCounterIsRegistred()
        {
            var application = new WebApplicationFactory<Program>();
            var serviceCounter = application.Services.GetService(typeof(IServiceCounter));
            Assert.IsNotNull(serviceCounter);
        }

        [TestCase("/")]
        [TestCase("/Index")]
        public async Task StartApplication_DoesNotError(string url)
        {
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}