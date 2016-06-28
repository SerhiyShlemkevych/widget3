using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using widget3.Code;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Concrete.Common;

namespace widget3.ViewModels.Concrete.SettingsWindow
{
    class WeatherStepViewModel : ViewModel
    {
        private string _apiKey = "IZuc0yoX7jiMhz2odbByTVDniOVUbQX2";
        private string _searchQuery = "http://dataservice.accuweather.com/locations/v1/cities/autocomplete?q={0}&apikey={1}";

        private string _searchText;

        private WeatherTileViewModel _tile;

        private WeatherSearchResult _selectedCity;

        public WeatherSearchResult SelectedCity
        {
            get
            {
                return _selectedCity;
            }
            set
            {
                _selectedCity = value;
                OnPropertyChanged("SelectedCity");

                Tile.Data = SelectedCity;
            }
        }


        public WeatherStepViewModel(WeatherTileViewModel tile)
        {
            _tile = tile;
            SearchResults = new ObservableCollection<WeatherSearchResult>();
        }

        public WeatherTileViewModel Tile
        {
            get
            {
                return _tile;
            }
        }

        public async Task Search()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return;
            }
            if (SelectedCity != null)
            {
                return;
            }           

            string url = string.Format(_searchQuery, HttpUtility.UrlEncode(SearchText), _apiKey);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response =  await request.GetResponseAsync();

            dynamic[] result;

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var data = await reader.ReadToEndAsync();
                result = JsonConvert.DeserializeObject<dynamic[]>(data);
            }

            SearchResults.Clear();

            foreach (var item in result)
            {
                var searchResult = new WeatherSearchResult()
                {
                    City = item.LocalizedName,
                    Country = item.Country.LocalizedName,
                    Key = item.Key
                };

                SearchResults.Add(searchResult);
            }

        }

        public ObservableCollection<WeatherSearchResult> SearchResults
        {
            get;
            private set;
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                Search();
            }
        }
    }
}
