using Core.Models;

namespace Core.Service
{
    public struct APIResponse
    {
        public WeatherInfo WeatherInfo;
        public Error error;
    }
}
