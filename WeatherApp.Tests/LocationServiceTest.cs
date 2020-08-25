using NUnit.Framework;
using MvvmCross.Tests;
using Core.Service;
using Moq;
using System.Threading.Tasks;

namespace Weather.tests
{
    [TestFixture]
    public class TestLocationService : MvxIoCSupportingTest
    {
        private Mock<ILocationService> _locationService;

        protected override void AdditionalSetup()
        {
            _locationService = new Mock<ILocationService>();
            Ioc.RegisterSingleton<ILocationService>(_locationService.Object);
        }

        //[TearDown]
        //public void Cleanup()
        //{

        //}


        [Test]
        public async Task Test_Fetch_City_Name_Async()
        {
            /*Task.Run(async () =>
            {
                string cityName = await _locationService.Object.getCityNameAsync(lat: 51.5074, lon: 0.1278);
                Assert.True(string.IsNullOrEmpty(cityName) == true, "Geo coding Failed.");
                Assert.True(cityName == "Landon", "Geo coding Failed.");
            }
            ).GetAwaiter().GetResult();*/
            string cityName = await _locationService.Object.getCityNameAsync(lat: 51.5074, lon: 0.1278);
                Assert.True(string.IsNullOrEmpty(cityName) == true, "Geo coding Failed.");
                Assert.True(cityName == "Landon", "Geo coding Failed.");
        }
    }
}
