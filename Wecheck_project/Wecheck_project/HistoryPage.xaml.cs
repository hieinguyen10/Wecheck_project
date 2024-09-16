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
    public partial class HistoryPage : ContentPage
    {
        ObservableCollection<Location> locationHistory = new ObservableCollection<Location>();
        DataAccess dataAccess;
        public HistoryPage() 
        {
            InitializeComponent();
           
        }
        
            

        protected override void OnAppearing()
        {   
            base.OnAppearing();
            dataAccess = new DataAccess(App.DatabaseLocation);
            locationHistory = new ObservableCollection<Location>(dataAccess.ReverseDataList());
            historyView.ItemsSource = locationHistory;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            ImageButton button = sender as ImageButton;
            StackLayout stackLayout = button.Parent as StackLayout;
            Label label = stackLayout.Children[1] as Label;
            string name = label.Text;
            foreach(var item in locationHistory)
            {
                if (item.location == name)
                {
                    if (item.isFavorite == true)
                    {
                        button.Source = "false_icon";
                    }
                    else
                    {
                        button.Source = "true_icon";
                    }
                    dataAccess.editFavorite(item.location);
                    break;
                }
            }
            
            
            
            

        }

        private void Delete_Clicked_1(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var location = menuItem.CommandParameter as Location;
            locationHistory.Remove(location);
            dataAccess.removeLocation(location);
            
        }

        private async void historyView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listHis = sender as ListView;
            var locaiton = listHis.SelectedItem as Location;
            await Navigation.PushAsync(new WeatherPage(locaiton.location));

        }
    }
}