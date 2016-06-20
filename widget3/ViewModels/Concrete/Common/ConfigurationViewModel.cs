using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class ConfigurationViewModel : ViewModel
    {
        private int _tileSize;
        private int _tileMargin;
        private int _fontSize;
        private int _subFontSize;

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
    }
}
