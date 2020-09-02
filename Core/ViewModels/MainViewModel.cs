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
        private IDialogService DialogService { get; }

        #region ICommands
        private readonly MvxCommand<string> _searchCommand;
        public MvxCommand<string> SearchCommand => _searchCommand;

        private readonly MvxCommand _searchCurrentWeather;
        public MvxCommand SearchCurrentWeatherCommand => _searchCurrentWeather;
        #endregion

        #region Binding variable
        private bool _isServiceRunning = false;
        public Boolean IsServiceRunning
        {
            get => _isServiceRunning;
            set => _ = SetProperty(ref _isServiceRunning, value);
        }

        WeatherInfo _weather = new WeatherInfo();
        public WeatherInfo WeatherObj
        {
            get => _weather;
            set => _ = SetProperty(ref _weather, value);
        }

        private WeatherCondition _weatherForecast;
        public WeatherCondition WeatherForecast
        {
            get => _weatherForecast;
            set => _ = SetProperty(ref _weatherForecast, value);
        }

        private ImageSource _weatherImage = "cloudiness.png";
        public ImageSource WeatherImage
        {
            get => _weatherImage;
            set => _ = SetProperty(ref _weatherImage, value);
        }

        #endregion

        // TODO: you should add tests for this ViewModel
        public MainViewModel(IWeatherService _weatherService, ILocationService _locationService,
            IImageService _imageService, IDialogService _dialogService)
        {
            WeatherService = _weatherService;
            LocationService = _locationService;
            ImageService = _imageService;
            DialogService = _dialogService;
            _searchCommand = new MvxCommand<string>(SearchButtonClicked);
            _searchCurrentWeather = new MvxCommand(SearchCurrentWeatherButtonClicked);
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
            await SearchByCurrentLocation();
        }

        #region command implementation

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
            else
            {
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

        #endregion
    }
}
