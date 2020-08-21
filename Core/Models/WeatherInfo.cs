using System;
namespace Core.Models
{
    public class WeatherInfo
    {
        public WeatherInfo()
        {
        }

        public WeatherCondition[] Weather { get; set; }
        public WeatherMain Main {get; set;}
        public int Visibility { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public long Dt { get; set; }
        public string Name { get; set; }

        public string Date
        {
            get
            {
                DateTime t;
                if (Dt == 0)
                {
                    //get current time
                    t = new DateTime();
                }
                else
                {
                    t = new DateTime().ToUniversalTime().AddSeconds(Dt);
                }
               return t.ToString("dddd, MMM dd").ToUpper();
            }
        }
    }
}
