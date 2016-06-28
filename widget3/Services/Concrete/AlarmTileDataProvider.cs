using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using widget3.Code;
using widget3.Services.Abstract;
using widget3.ViewModels.Concrete.Common;

namespace widget3.Services.Concrete
{
    public class AlarmTileDataProvider : TileDataProvider<AlarmTileViewModel>
    {
        MediaPlayer _player;

        public AlarmTileDataProvider(IUserDataService userData) : base(userData)
        {
            _player = new MediaPlayer();
            _player.Open(new Uri("resources/sound/alarm.mp3", UriKind.Relative));
            _timers = new Dictionary<AlarmTileViewModel, Timer>();
            Tiles.CollectionChanged += TilesCollectionChanged;
            RefreshAllTiles();
        }

        private void TilesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach(AlarmTileViewModel tile in e.OldItems)
                {
                    if (_timers.Keys.Contains(tile))
                    {
                        _timers[tile].Dispose();
                        _timers.Remove(tile);
                    }
                }
            }
        }

        protected override void OnTilePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsAlarmDisabled")
            {
                var tile = sender as AlarmTileViewModel;
                if (tile.IsAlarmDisabled)
                {
                    if (_timers.Keys.Contains(tile))
                    {
                        _timers[tile].Dispose();
                        _timers.Remove(tile);
                    }
                }
                else
                {
                    var now = new SmallTime(DateTime.Now.Hour, DateTime.Now.Minute);
                    var interval = ((SmallTime)tile.Data).Milliseconds - now.Milliseconds;
                    if (interval < 0) return;
                    if (_timers.Keys.Contains(tile))
                    {
                        _timers[tile].Change(interval, Timeout.Infinite);
                    }
                    else
                    {
                        _timers.Add(tile, new Timer(TimerCallback));
                        _timers[tile].Change(interval, Timeout.Infinite);
                    }
                }
            }
        }

        private void TimerCallback(object state)
        {
            _player.Play();
        }

        private Dictionary<AlarmTileViewModel, Timer> _timers;

        protected override void ProvideTileWithValue(AlarmTileViewModel tile)
        {
            var now = new SmallTime(DateTime.Now.Hour, DateTime.Now.Minute);
            var time = ((SmallTime)tile.Data).Milliseconds - now.Milliseconds;
            if (time >= 0)
            {
                if (_timers.Keys.Contains(tile))
                {
                    _timers[tile].Change(time, Timeout.Infinite);
                }
                else
                {
                    _timers.Add(tile, new Timer(TimerCallback));
                    _timers[tile].Change(time, Timeout.Infinite);
                }
            }

            tile.Text = tile.Data.ToString();
        }
    }
}
