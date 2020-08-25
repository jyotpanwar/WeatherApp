using NUnit.Framework;
using MvvmCross.Tests;
using Core.Service;
using Moq;
using System.Threading.Tasks;

namespace WeatherApp.Tests
{
    public class WeatherServiceTest : MvxIoCSupportingTest
    {
        private Mock<IWeatherService> _weatherService;

        protected override void AdditionalSetup()
        {
            _weatherService = new Mock<IWeatherService>();
            Ioc.RegisterSingleton<IWeatherService>(_weatherService.Object);
        }

        [Test]
        public void Test_Fetch_Weather_()
        {
            Task.Run(async () =>
            {
                APIResponse results = await _weatherService.Object.FetchWeatherDataAsync("Landon");
                System.Console.WriteLine("TEST: API RESPONSE" + results);
                Assert.True((results.error.Message == null) && (results.WeatherInfo.Name == null), "Weather Data not found.");
            }
            ).GetAwaiter().GetResult();
        }
    }
}
