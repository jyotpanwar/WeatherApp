using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.IO;

namespace Core.Service
{

    public interface IImageService
    {
        public Task<ImageSource> DownloadIconAsync(string iconName);
    }

    public class ImageService : IImageService
    {
        static readonly string IconUrl = "https://openweathermap.org/img/wn/{0}@2x.png";

        public ImageService()
        {
        }

        public async Task<ImageSource> DownloadIconAsync(string iconName)
        {
            ImageSource imageSource = "cloudiness.png"; // store default Image
            HttpClient httpClient = new HttpClient();
            var uriString = string.Format(IconUrl, iconName);
            var uri = new Uri(uriString: uriString);
            var response = await httpClient.GetByteArrayAsync(uri);
            if (response != null)
            {
                imageSource = ImageSource.FromStream(() => new MemoryStream(response));
            }
            return imageSource;
        }
    }
}
