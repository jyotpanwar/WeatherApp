using System;
using Xamarin.Essentials;
using MvvmCross;
using System.Threading.Tasks;
using System.Linq;

namespace Core.Service
{
    public interface ILocationService
    {
        public Task<Location> GetCurrentLocationAsync();
        public Task<String> getCityNameAsync(double lat, double lon);
    }

    public class LocationService : ILocationService
    {
        public LocationService()
        {
            
        }

        public async Task<string> getCityNameAsync(double lat, double lon)
        {
            var cityName = "";

            if (lat != 0 && lon != 0)
            {
                try
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        cityName = placemark.Locality;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw ex;
                }
            }
            
            return cityName;
        }

        public async Task<Location> GetCurrentLocationAsync()
        {
            Location location;
            try
            {
                location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }

            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("errorrrr: {0}", ex);
                throw ex;
            }

            Console.WriteLine("Locationnn : {0}", location);
            return location;
        }
    }
}
