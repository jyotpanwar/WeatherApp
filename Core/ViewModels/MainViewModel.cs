using System;
using MvvmCross.ViewModels;
using Core.Service;
using Core.Models;
using MvvmCross.Commands;
using System.Linq;
using Xamarin.Forms;

namespace Weather.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IWeatherService WeatherService { get; }
        private ILocationService LocationService { get; }
        private IImageService ImageService { get; }

        MvxCommand<string> _searchCommand;
        public MvxCommand<string> SearchCommand
        {
            get => new MvxCommand<string>(SearchButtonClicked);

            set => _searchCommand = value;
        }

        WeatherInfo _weather = new WeatherInfo();
        public WeatherInfo WeatherObj
        {
            get
            {
                return _weather;
            }
            set
            {
                _weather = value;
                RaisePropertyChanged(() => WeatherObj);
            }
        }

        private WeatherCondition _weatherForecast;
        public WeatherCondition WeatherForecast
        {
            get
            {
                return _weatherForecast;
            }
            set
            {
                _weatherForecast = value;
                RaisePropertyChanged(() => WeatherForecast);
            }
        }

        private ImageSource _weatherImage = "cloudiness.png";
        public ImageSource WeatherImage
        {
            get
            {
                return _weatherImage;
            }
            set
            {
                _weatherImage = value;
                RaisePropertyChanged(() => WeatherImage);
            }
        }


        public MainViewModel(IWeatherService _weatherService, ILocationService _locationService, IImageService _imageService)
        {
            WeatherService = _weatherService;
            LocationService = _locationService;
            ImageService = _imageService;
        }

        private async void SearchButtonClicked(string Text)
        {
            //search weather of city entered
            if (string.IsNullOrEmpty(Text))
            {
                //show alert
            }
            else {
                try
                {
                    WeatherObj = await WeatherService.FetchWeatherDataAsync(city: Text);
                    WeatherForecast = WeatherObj.Weather.FirstOrDefault();
                    SetWeatherImage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
            try
            {
                var location = await LocationService.GetCurrentLocationAsync();
                var city = await LocationService.getCityNameAsync(location.Latitude, location.Longitude);
                if (!string.IsNullOrEmpty(city))
                {
                    WeatherObj = await WeatherService.FetchWeatherDataAsync(city: city);
                    WeatherForecast = WeatherObj.Weather.FirstOrDefault();
                    SetWeatherImage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async void SetWeatherImage()
        {
            WeatherImage = await ImageService.DownloadIconAsync(WeatherForecast.Icon);
        }
    }
}
