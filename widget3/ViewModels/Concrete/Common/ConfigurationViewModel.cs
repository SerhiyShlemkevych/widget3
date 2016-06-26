using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class ConfigurationViewModel : ViewModel
    {
        private int _tileSize;
        private int _tileMargin;
        private int _fontSize;
        private int _subFontSize;
        private double _fontTransperency;
        private int _currentDay;

        public ConfigurationViewModel()
        {
            ChangeCurrentDay((int)DateTime.Now.DayOfWeek);
            TileHeights = new List<int>() { 1, 2, 3, 4 };
            TileWidths = new List<int>() { 1, 2, 3, 4 };
        }

        public double FontTransperency
        {
            get
            {
                return _fontTransperency;
            }
            set
            {
                _fontTransperency = value;
                OnPropertyChanged("FontTransperency");
            }
        }

        public List<int> TileHeights
        {
            get;
            private set;
        }

        public List<int> TileWidths
        {
            get;
            private set;
        }

        public int CurrentDay
        {
            get
            {
                return _currentDay;
            }
        }



        public int RowCount
        {
            get
            {
                return WindowHeight / TileSize;
            }
            set
            {
            }
        }

        public int ColumnCount
        {
            get
            {
                return WindowWidth / TileSize;
            }
            set
            {
            }
        }

        public int WindowWidth
        {
            get
            {
                return (int)SystemParameters.PrimaryScreenWidth;
            }
            set
            {
            }
        }

        public int WindowHeight
        {
            get
            {
                return (int)SystemParameters.PrimaryScreenHeight;
            }
            set
            {
            }
        }

        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        public int SubFontSize
        {
            get
            {
                return _subFontSize;
            }
            set
            {
                _subFontSize = value;
                OnPropertyChanged("SubFontSize");
            }
        }

        public int TileSize
        {
            get
            {
                return _tileSize;
            }
            set
            {
                _tileSize = value;
                OnPropertyChanged("TileSize");
                OnPropertyChanged("RowCount");
                OnPropertyChanged("ColumnCount");
            }
        }

        public int TileMargin
        {
            get
            {
                return _tileMargin;
            }
            set
            {
                _tileMargin = value;
                OnPropertyChanged("TileMargin");
            }
        }

        public event Action<object, EventArgs> CurrentDayChanged;

        public void ChangeCurrentDay(int day)
        {
            if (day < 0 || day > 6)
            {
                throw new ArgumentOutOfRangeException("day");
            }

            _currentDay = day;
            if (CurrentDayChanged != null)
            {
                CurrentDayChanged(this, EventArgs.Empty);
            }
        }
    }
}
