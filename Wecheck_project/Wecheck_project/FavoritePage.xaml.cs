using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wecheck_project.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wecheck_project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePage : ContentPage
    {
       
        ObservableCollection<FavoriteLocation> locationFavorites = new ObservableCollection<FavoriteLocation>();
        DataAccess dataAccess;
        private string APIkey = "7578832a410ae3789407cb6513523cfd";

        public FavoritePage()
        {
            InitializeComponent();
            GetWeatherList();
            favoriteView.ItemsSource = locationFavorites;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            locationFavorites = new ObservableCollection<FavoriteLocation>();
            GetWeatherList();
            favoriteView.ItemsSource = locationFavorites;
        }
        
        private void GetWeatherList()
        {
            dataAccess = new DataAccess(App.DatabaseLocation);
            var locationFav = new List<Location>(dataAccess.GetFavoriteData());
            foreach(var location in locationFav)
            {
                GetWeatherInfo(location.location);
            }
        }
        private async void GetWeatherInfo(string Location)
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid={APIkey}";

            var result = await ApiCaller.Get(url);
            FavoriteLocation favLocation = new FavoriteLocation();
            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    favLocation = new FavoriteLocation()
                    {
                        name = Location,
                        temp = (weatherInfo.main.temp - 273.15).ToString("0") + "°",
                        weatherIcon = $"{weatherInfo.weather[0].main}_icon.png"
                    };
                    locationFavorites.Add(favLocation);
                    
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

        private void Remove_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var location = menuItem.CommandParameter as FavoriteLocation;
            locationFavorites.Remove(location);
            dataAccess.editFavorite(location.name);
        }

        private async void favoriteView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listFav = sender as ListView;
            var locaiton = listFav.SelectedItem as FavoriteLocation;
            await Navigation.PushAsync(new WeatherPage(locaiton.name));
        }
    }
}