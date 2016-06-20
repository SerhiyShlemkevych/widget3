using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using widget3.Controls.Abstract.Common;
using widget3.Enums;
using widget3.Services.Abstract;
using widget3.ViewModels.Concrete.Common;

namespace widget3.ViewModels.Abstract.Common
{
    public abstract class TileViewModel : ViewModel
    {
        private string _text;
        private int _height;
        private int _width;
        private int _row;
        private int _column;
        private BackgroundViewModel _background;
        private TileType _type;
        private object _data;
        private ICommand _command;
        private object _commandParameter;

        private IUserDataService _userData;

        public TileViewModel(IUserDataService userData)
        {
            _userData = userData;
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;

                OnPropertyChanged("Height");
            }
        }
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        //public int Width
        //{
        //    get
        //    {
        //        return WidthMultipler * App.SharedData.Configuration.TileSize;
        //    }
        //}

        //public int Height
        //{
        //    get
        //    {
        //        return HeightMultipler * App.SharedData.Configuration.TileSize;
        //    }
        //}


        public object Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged("Data"); }
        }
        public BackgroundViewModel Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
            }
        }

        public TileType Type
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

        public int Row
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
                OnPropertyChanged("Row");
            }
        }

        public int Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
                OnPropertyChanged("Column");
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

        protected virtual void SetBindings(TileBase tileView)
        {
            Binding background = new Binding("Background.BackgroundData");
            Binding text = new Binding("Text");
            Binding command = new Binding("Command");
            Binding commandParaameter = new Binding("CommandParameter");
            Binding width = new Binding("Width");
            Binding height = new Binding("Height");
            tileView.SetBinding(TileBase.WidthProperty, width);
            tileView.SetBinding(TileBase.HeightProperty, height);
            tileView.SetBinding(TileBase.BackgroundProperty, background);
            tileView.SetBinding(TileBase.TextProperty, text);
            tileView.SetBinding(TileBase.CommandProperty, command);
            tileView.SetBinding(TileBase.CommandParameterProperty, commandParaameter);

            Binding fontSize = new Binding("FontSize");
            fontSize.Source = _userData.Configuration;
            tileView.SetBinding(TileBase.FontSizeProperty, fontSize);

            Binding subFontSize = new Binding("SubFontSize");
            subFontSize.Source = _userData.Configuration;
            tileView.SetBinding(TileBase.SubFontSizeProperty, subFontSize);

            Binding margin = new Binding("TileMargin");
            margin.Source = _userData.Configuration;
            tileView.SetBinding(TileBase.MarginProperty, margin);
        }

        public abstract TileBase CreateTileView();
    }
}

