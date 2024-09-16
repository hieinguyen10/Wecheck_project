using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using SQLite;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace Wecheck_project.Models
{
    public class DataAccess
    {
        SQLiteConnection database;
        List<Location> locations;

        public DataAccess(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Location>(); // Create table if not exists
            locations = new List<Location>(database.Table<Location>().ToList());
        }
        public bool checkLocationExistance(Location locationCheck)
        {
            return locations.Any(location => location.location == locationCheck.location);
        }
        public List<Location> GetAllData()
        {
            return locations;
        }
        public ObservableCollection<Location> GetFavoriteData()
        {
            ObservableCollection<Location> listFavorite = new ObservableCollection<Location>();
            foreach (var location in locations) 
            {
                if(location.isFavorite == true)
                {
                    listFavorite.Add(location);
                }
            }
            return listFavorite;
        }
        public int countNonFavLocation() 
        { 
            int count = 0;
            foreach (var item in locations)
            {
                if(item.isFavorite == false) 
                {
                    count++;
                }
            }
            return count;
        }
        public List<Location> ReverseDataList()
        {
            int count = 0;
            List<Location> reverseList = new List<Location>();
            for(int i = locations.Count - 1; i >= 0; i--)
            {
                count++;
                reverseList.Add(locations[i]);
                if (count == 5)
                {
                    break;
                }
            }
            return reverseList;
        }
        public void insertData(Location location)
        {
            if (checkLocationExistance(location)==true)
            {
                database.Delete(location);
                database.Insert(location);
            }
            else
            {
                int countNonFav = countNonFavLocation();
                if (countNonFav < 5)
                {
                    database.Insert(location);
                }
                else
                {
                    while(countNonFav >= 5)
                    {
                        foreach (var item in locations) 
                        {
                            if(item.isFavorite == false)
                            {
                                database.Delete(item);
                                countNonFav--;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public void editFavorite(string name)
        {
            foreach(var item in locations)
            {
                if (item.location == name)
                {
                    if (item.isFavorite == false)
                    {
                        item.isFavorite = true;
                    }
                    else
                    {
                        item.isFavorite = false;
                    }
                    database.Update(item);
                    break;
                }    
            }
           
        }

        public void removeLocation(Location location)
        {
            database.Delete(location);
        }

    }
}
