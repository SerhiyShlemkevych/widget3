﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using widget3.Code;
using widget3.Controls.Abstract.Common;
using widget3.Converters;
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
        private ICommand _command;
        private object _commandParameter;
        private Thickness _borderThickness;
        private TileData _data;
        private bool[] _days;
        private double _transperency = 100;
        Regex colorRegex = new Regex(@"\#.{8}$");

        public TileViewModel()
        {
            Days = new bool[7];
            Data = new TileData();
            SetDefaultCommand();
        }

        public Brush BackgroundBrush
        {
            get
            {
               
                if (Background.Type == BackgroundType.SolidColor)
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(Background.BackgroundData));
                }
                else
                {
                    return new ImageBrush(GetCroppedImage());
                }
            }
            set
            {
            }
        }

        public double Transperency
        {
            get
            {
                return _transperency;
            }
            set
            {
                _transperency = value;
                OnPropertyChanged("Transperency");
            }
        }

        public bool[] Days
        {
            get
            {
                return _days;
            }
            set
            {
                _days = value;
                OnPropertyChanged("Days");
            }
        }

        public bool Monday
        {
            get
            {
                return Days[1];
            }
            set
            {
                Days[1] = value;
                OnPropertyChanged("Monday");
                OnPropertyChanged("Days");
            }
        }

        public bool Tuesday
        {
            get
            {
                return Days[2];
            }
            set
            {
                Days[2] = value;
                OnPropertyChanged("Tuesday");
                OnPropertyChanged("Days");
            }
        }

        public bool Wednesday
        {
            get
            {
                return Days[3];
            }
            set
            {
                Days[3] = value;
                OnPropertyChanged("Wednesday");
                OnPropertyChanged("Days");
            }
        }

        public bool Thursday
        {
            get
            {
                return Days[4];
            }
            set
            {
                Days[4] = value;
                OnPropertyChanged("Thursday");
                OnPropertyChanged("Days");
            }
        }

        public bool Friday
        {
            get
            {
                return Days[5];
            }
            set
            {
                Days[5] = value;
                OnPropertyChanged("Friday");
                OnPropertyChanged("Days");
            }
        }

        public bool Saturday
        {
            get
            {
                return Days[6];
            }
            set
            {
                Days[6] = value;
                OnPropertyChanged("Saturday");
                OnPropertyChanged("Days");
            }
        }

        public bool Sunday
        {
            get
            {
                return Days[0];
            }
            set
            {
                Days[0] = value;
                OnPropertyChanged("Sunday");
                OnPropertyChanged("Days");
            }
        }

        public IUserDataService UserData
        {
            get;
            set;
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
                if (Background?.Type == BackgroundType.Image) OnPropertyChanged("BackgroundBrush");
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
                if (Background?.Type == BackgroundType.Image) OnPropertyChanged("BackgroundBrush");
            }
        }
        public TileData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
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
                OnPropertyChanged("BackgroundBrush");
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

        public void Select()
        {
            BorderThickness = new Thickness(5);
        }

        public void Deselect()
        {
            BorderThickness = new Thickness(0);
        }

        public abstract void SetDefaultCommand();

        protected virtual void SetBindings(TileBase tileView)
        {
            Binding fontSize = new Binding("FontSize");
            fontSize.Source = UserData.Configuration;
            tileView.SetBinding(TileBase.FontSizeProperty, fontSize);

            Binding fontTransperency = new Binding("FontTransperency");
            fontTransperency.Source = UserData.Configuration;
            tileView.SetBinding(TileBase.FontTransperencyProperty, fontTransperency);

            Binding subFontSize = new Binding("SubFontSize");
            subFontSize.Source = UserData.Configuration;
            tileView.SetBinding(TileBase.SubFontSizeProperty, subFontSize);

            Binding margin = new Binding("TileMargin");
            margin.Source = UserData.Configuration;
            tileView.SetBinding(TileBase.MarginProperty, margin);
        }

        private ImageSource GetCroppedImage()
        {
            BitmapImage image = new BitmapImage(new Uri(Background.BackgroundData, UriKind.Absolute));
            CroppedBitmap croppedImage = null;

            int height = 0;
            int width = 0;

            if (Width > Height)
            {
                width = image.PixelWidth;
                height = width * Height / Width;
            }
            else
            {
                height = image.PixelHeight;
                width = height * Width / Height;
            }

            Int32Rect rectangle = new Int32Rect(0, 0, width, height);
            croppedImage = new CroppedBitmap(image, rectangle);
            return croppedImage;
        }

        public abstract IEnumerable<TileEditPropertyInfo> GetEditInfo();

        public abstract IEnumerable<CreateTileStep> GetCreateSteps();

        public abstract TileBase CreateTileView();
    }
}

