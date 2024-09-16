using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wecheck_project.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wecheck_project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage
    {
        List<Forecast> forecasts = new List<Forecast>();
        private string Location = "Montreal";
        private string APIkey = "7578832a410ae3789407cb6513523cfd";
        public WeatherPage()
        {
            InitializeComponent();
            GetWeatherInfo();
            GetForecast();
        }
        public WeatherPage(string value)
        {
            InitializeComponent();
            Location = value;
            GetWeatherInfo();
            GetForecast();
        }

        private async void GetWeatherInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid={APIkey}";

            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    imgWeather.Source = $"{weatherInfo.weather[0].main}_icon.png";
                    //imgWeather.Source = $"{weatherInfo.weather[0].icon}";
                    lblLocation.Text = weatherInfo.name.ToUpper();
                    lblTemperature.Text = (weatherInfo.main.temp-273.15).ToString("0") + "°";
                    lblHumidity.Text = $"{weatherInfo.main.humidity}%";
                    lblWindSpeed.Text = $"{weatherInfo.wind.speed}";
                    lblFeelLike.Text = (weatherInfo.main.feels_like - 273.15).ToString("0")+ "°";


                    DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.dt).UtcDateTime;
                    lblDate.Text = dateTime.ToString("dddd, MMM dd").ToUpper();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
                
            }
            else
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }
        private async void GetForecast()
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q={Location}&appid={APIkey}";
            var result = await ApiCaller.Get(url);
            

            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    
                    List<List> listWeather = new List<List>();

                    foreach (var list in forcastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            listWeather.Add(list);
                    }

                    imgForecast_1.Source = $"{listWeather[0].weather[0].main}_icon.png";
                    lblDate_1.Text = DateTime.Parse(listWeather[0].dt_txt).ToString("dddd");
                    lblTemp_1.Text = ((listWeather[0].main.temp)-273.15).ToString("0")+ "°C";

                    imgForecast_2.Source = $"{listWeather[1].weather[0].main}_icon.png";
                    lblDate_2.Text = DateTime.Parse(listWeather[1].dt_txt).ToString("dddd");
                    lblTemp_2.Text = ((listWeather[1].main.temp) - 273.15).ToString("0") + "°C";

                    imgForecast_3.Source = $"{listWeather[2].weather[0].main}_icon.png";
                    lblDate_3.Text = DateTime.Parse(listWeather[2].dt_txt).ToString("dddd");
                    lblTemp_3.Text = ((listWeather[2].main.temp) - 273.15).ToString("0") + "°C";

                    imgForecast_4.Source = $"{listWeather[3].weather[0].main}_icon.png";
                    lblDate_4.Text = DateTime.Parse(listWeather[3].dt_txt).ToString("dddd");
                    lblTemp_4.Text = ((listWeather[3].main.temp) - 273.15).ToString("0") + "°C";

                    imgForecast_5.Source = $"{listWeather[4].weather[0].main}_icon.png";
                    lblDate_5.Text = DateTime.Parse(listWeather[4].dt_txt).ToString("dddd");
                    lblTemp_5.Text = ((listWeather[4].main.temp) - 273.15).ToString("0") + "°C";

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
               
            }
            else
            {
                await DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }
    }
}