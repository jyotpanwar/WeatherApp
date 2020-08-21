using System;
namespace Core.Models
{
    public class WeatherMain
    {
        public WeatherMain()
        {
        }

        public int Pressure { get; set; }
        public double Temp { get; set; }
        public int Humidity { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
    }
}
