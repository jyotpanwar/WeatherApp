using System;
using MvvmCross.ViewModels;
using Core.Service;
using Core.Models;
using MvvmCross.Commands;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Weather.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private IWeatherService WeatherService { get; }
        private ILocationService LocationService { get; }
        private IImageService ImageService { get; }
        private IDialogService DialogService { get;  }

        private bool _isServiceRunning = false;
        public Boolean IsServiceRunning
        {
            get => _isServiceRunning;
            set
            {
                _isServiceRunning = value;
                RaisePropertyChanged(()=> IsServiceRunning);
            }
        }


        MvxCommand<string> _searchCommand;
        public MvxCommand<string> SearchCommand
        {
            // TODO: you are creating a new command every time the getter is accessed,
            // rather instantiate in constructor or do lazy instantiation here
            get => new MvxCommand<string>(SearchButtonClicked);

            set => _searchCommand = value;
        }

        MvxCommand _searchCurrentWeather;
        public MvxCommand SearchCurrentWeatherCommand
        {
            get
            {
                // TODO: see comments above
                return new MvxCommand(SearchCurrentWeatherButtonClicked);
            }
            set => _searchCurrentWeather = value;
        }

        public async void SearchCurrentWeatherButtonClicked()
        {
            await SearchByCurrentLocation();
        }

        private async Task SearchByCityName(string city)
        {
            var apiResponse = await WeatherService.FetchWeatherDataAsync(city: city);
            if (apiResponse.WeatherInfo != null)
            {
                WeatherObj = apiResponse.WeatherInfo;
                if (WeatherObj != null)
                {
                    WeatherForecast = WeatherObj.Weather.FirstOrDefault();
                    SetWeatherImage();
                }
            }
            else if (!string.IsNullOrEmpty(apiResponse.error.Message))
            {
                DialogService.ShowMessage(title: "Error",
                message: apiResponse.error.Message,
                dismissButtonTitle: "Ok", dismissed: null);
            }
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
                // TODO : use SetProperty()
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
                // TODO : use SetProperty()
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
                // TODO : use SetProperty()
                _weatherImage = value;
                RaisePropertyChanged(() => WeatherImage);
            }
        }

        // TODO: you should add tests for this ViewModel
        public MainViewModel(IWeatherService _weatherService, ILocationService _locationService,
            IImageService _imageService, IDialogService _dialogService)
        {
            WeatherService = _weatherService;
            LocationService = _locationService;
            ImageService = _imageService;
            DialogService = _dialogService;
        }

        private async void SearchButtonClicked(string Text)
        {
            //search weather of city entered
            if (string.IsNullOrEmpty(Text))
            {
                //show alert
                DialogService.ShowMessage(title: "Error",
                    message: "Please enter a city name.",
                    dismissButtonTitle: "Ok", dismissed: null);
            }
            else {
                try
                {
                    IsServiceRunning = true;
                    await SearchByCityName(city: Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    IsServiceRunning = false;
                    DialogService.ShowMessage(title: "Error",
                    message: ex.ToString(),
                    dismissButtonTitle: "Ok", dismissed: null);
                }
                IsServiceRunning = false;
            }
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
            await SearchByCurrentLocation();
        }

        private async Task SearchByCurrentLocation()
        {
            try
            {
                IsServiceRunning = true;
                var location = await LocationService.GetCurrentLocationAsync();
                var city = await LocationService.getCityNameAsync(location.Latitude, location.Longitude);
                if (!string.IsNullOrEmpty(city))
                {
                    await SearchByCityName(city: city);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                IsServiceRunning = false;
                DialogService.ShowMessage(title: "Error",
                    message: ex.ToString(),
                    dismissButtonTitle: "Ok", dismissed: () => { });
            }
            IsServiceRunning = false;
        }

        public async void SetWeatherImage()
        {
            WeatherImage = await ImageService.DownloadIconAsync(WeatherForecast.Icon);
        }
    }
}
