using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.SettingsWindow;
using widget3.Converters;
using widget3.Enums;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class BackgroundViewModel : ViewModel
    {
        private BackgroundType _type;
        private ICommand _command;
        private string _backgroundData;
        private Thickness _borderThickness;
        private object _commandParameter;

        public object CommandParameter
        {
            get
            {
                return _commandParameter;
            }
            set
            {
                _commandParameter = value;
                OnPropertyChanged("CommandParameter");
            }
        }

        public ICommand Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
                OnPropertyChanged("Command");
            }
        }

        public BackgroundType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public string BackgroundData
        {
            get
            {
                return _backgroundData;
            }
            set
            {
                _backgroundData = value;
                OnPropertyChanged("BackgroundData");
            }
        }

        public Thickness BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                _borderThickness = value;
                OnPropertyChanged("BorderThickness");
            }
        }

        public void Select()
        {
            BorderThickness = new Thickness(1);
        }

        public void Deselect()
        {
            BorderThickness = new Thickness(0);
        }

        public TileBase CreateTileView()
        {
            var view = new BackgroundTile() { DataContext = this };
            return view;
        }
    }
}
