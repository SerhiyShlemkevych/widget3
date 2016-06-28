using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using widget3.Services.Abstract;
using widget3.ViewModels.Concrete.Common;

namespace widget3.Services.Concrete
{
    public class DateTimeTileDataProvider : TileDataProvider<DateTimeTileViewModel>
    {
        private Timer _refreshTimer;

        public DateTimeTileDataProvider(IUserDataService userData) : base(userData)
        {
            _refreshTimer = new Timer(TimerCallBack);
            RefreshAllTiles();
            _refreshTimer.Change(30000, Timeout.Infinite);
        }

        protected override void ProvideTileWithValue(DateTimeTileViewModel tile)
        {
            var now = DateTime.Now;
            tile.Day = now.DayOfWeek.ToString();
            tile.Date = now.ToShortDateString();
            tile.Text = now.ToShortTimeString();
        }

        private void TimerCallBack(object state)
        {
            RefreshAllTiles();
            _refreshTimer.Change(30000, Timeout.Infinite);
        }
    }
}
