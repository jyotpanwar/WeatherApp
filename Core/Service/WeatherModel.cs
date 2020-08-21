using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Core.Models;
using System.Web;

namespace Core.Service
{
    public interface IWeatherService
    {
        public static string ApiKey;
        public static string ApiUrl;
        //public Task<WeatherInfo> FetchWeatherDataAsync(double latitide, double longitide);
        public Task<APIResponse> FetchWeatherDataAsync(string city);
    }


    public class WeatherService : IWeatherService
    {
        static readonly string ApiKey = "42b66f9753f35b296557e32c29cffdf6";
        static readonly string ApiUrl = $"https://api.openweathermap.org/data/2.5/weather?q=";
        
        public WeatherService()
        {
        }

        /*public async Task<WeatherModel> FetchWeatherDataAsync(double latitide, double longitide)
        {
            HttpClient httpClient = new HttpClient();
            var uriString = ApiUrl + String.Format(@"lat={0}&lon={1}&exclude=hourly,current,minutely&appid={2}", latitide, longitide, ApiKey);
            var uri = new Uri(uriString: uriString);
            var response = await httpClient.GetAsync(uri);

            var weatherJSON = response.Content.ReadAsStringAsync().Result;
            var weatherObject = JsonConvert.DeserializeObject<WeatherModel>(weatherJSON);

            return weatherObject;
        }*/

        public async Task<APIResponse> FetchWeatherDataAsync(string city)
        {
            HttpClient httpClient = new HttpClient();
            var htmlEncodedCity = HttpUtility.UrlEncode(city, System.Text.Encoding.UTF8);
            string uriString = ApiUrl + string.Format("{0}&appid={1}&units=metric", htmlEncodedCity, ApiKey);
            var uri = new Uri(uriString: uriString);
            Console.WriteLine("APIURL : " + uri);
            var response = await httpClient.GetAsync(uri);
            var weatherJSON = response.Content.ReadAsStringAsync().Result;
            WeatherInfo weatherObject = JsonConvert.DeserializeObject<WeatherInfo>(weatherJSON);

            var apiResponse = new APIResponse();
            if (weatherObject.Name == null)
            {
                apiResponse.error = JsonConvert.DeserializeObject<Error>(weatherJSON);
            }
            else
            {
                apiResponse.WeatherInfo = weatherObject;
            }

            return apiResponse;
        }
    }
}
