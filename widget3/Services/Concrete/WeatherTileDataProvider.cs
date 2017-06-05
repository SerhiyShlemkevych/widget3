using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using widget3.Code;
using widget3.Services.Abstract;
using widget3.ViewModels.Concrete.Common;

namespace widget3.Services.Concrete
{
    public class WeatherTileDataProvider : TileDataProvider<WeatherTileViewModel>
    {
        private string _apiKey = "IZuc0yoX7jiMhz2odbByTVDniOVUbQX2";
        private string _query = "http://dataservice.accuweather.com/currentconditions/v1/{0}?apikey={1}";
        private int _refreshTime = 300000;

        private Timer _refreshTimer;

        public WeatherTileDataProvider(IUserDataService userData) : base(userData)
        {
            _refreshTimer = new Timer(TimerCallBack);
            RefreshAllTiles();
            _refreshTimer.Change(_refreshTime, Timeout.Infinite);
        }

        protected override void ProvideTileWithValue(WeatherTileViewModel tile)
        {
            var url = string.Format(_query, ((WeatherSearchResult)tile.Data).Key, _apiKey);

            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                dynamic[] result = null;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string data = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<dynamic[]>(data);
                }

                foreach (var item in result)
                {
                    var rawTemperature = (string)item.Temperature.Metric.Value;
                    string temperature = string.Format("{0}°", rawTemperature.Split('.')[0]);
                    tile.Text = temperature;
                    tile.WeatherCondition = item.WeatherText;
                }
            }
            catch (WebException e)
            {
                tile.WeatherCondition = "No data";
                return;
            }
        }

        private void TimerCallBack(object state)
        {
            RefreshAllTiles();
            _refreshTimer.Change(_refreshTime, Timeout.Infinite);
        }
    }
}
