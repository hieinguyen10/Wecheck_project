using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wecheck_project.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using SQLitePCL;
using Newtonsoft.Json;

namespace Wecheck_project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private string APIkey = "7578832a410ae3789407cb6513523cfd";
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            string entryInput = entryLocation.Text;
            DataAccess dataAccess;
            var url = $"https://api.openweathermap.org/geo/1.0/direct?q={entryInput}&appid={APIkey}";

            var result = await ApiCaller.Get(url);
      
            if (result.Successful)
            {
                if (entryInput != null)
                {
                    try
                    {
                        var locationList = JsonConvert.DeserializeObject<List<Location>>(result.Response);
                        if(locationList.Count != 0)
                        {
                            var lo = locationList[0];
                            Location oneLocation = new Location()
                            {
                                location = entryInput,
                                isFavorite = false,
                            };
                            dataAccess = new DataAccess(App.DatabaseLocation);
                            dataAccess.insertData(oneLocation);
                            entryLocation.Text = null;
                        }
                        else
                        {
                            await DisplayAlert("Wrong", "Weather infor", "OK");
                        }
                        

                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Location Info", ex.Message, "OK");
                    }
                }

                await Navigation.PushAsync(new WeatherPage(entryInput));
            }
        }
    }
}