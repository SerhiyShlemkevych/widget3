using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace widget3.Code
{
    public class WeatherSearchResult : TileData
    {
        public string City
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", City, Country);
            }
        }
    }
}
