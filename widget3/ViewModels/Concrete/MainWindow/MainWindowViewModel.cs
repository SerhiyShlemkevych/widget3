using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Controls.Abstract.Common;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;
using System.Collections.Specialized;
using widget3.ViewModels.Concrete.Common;
using System.Windows.Media;
using System.Windows;
using widget3.Commands.Abstract;

namespace widget3.ViewModels.Concrete.MainWindow
{
    public class MainWindowViewModel : ViewModel
    {
        private IUserDataService _userData;

        private Brush _windowBackground;
        private widget3.Views.Main.MainWindow _window;

        public MainWindowViewModel(IUserDataService userData)
        {
            _userData = userData;
            TileViews = new ObservableCollection<TileBase>();
            userData.Configuration.CurrentDayChanged += ConfigurationCurrentDayChanged;
            InitializeTiles();

            WindowBackground = Brushes.Transparent;

            InitializeCommands();

            _window = new Views.Main.MainWindow() { DataContext = this };
            _window.Show();
        }

        private void ConfigurationCurrentDayChanged(object sender, EventArgs e)
        {
            foreach (var tile in _userData.Tiles)
            {
                EvaluateTile(tile);
            }
        }

        public Brush WindowBackground
        {
            get
            {
                return _windowBackground;
            }
            set
            {
                _windowBackground = value;
                OnPropertyChanged("WindowBackground");
            }
        }

        public DelegateCommand OpenSettingsWindowCommand
        {
            get;
            private set;
        }

        public DelegateCommand ExitCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TileBase> TileViews
        {
            get;
            private set;
        }

        public ConfigurationViewModel Configuration
        {
            get
            {
                return _userData.Configuration;
            }
        }

        private void InitializeTiles()
        {
            foreach (var tile in _userData.Tiles)
            {
                tile.PropertyChanged += TileDayPropertyChanged;
                EvaluateTile(tile);         
            }

            _userData.Tiles.CollectionChanged += TilesCollectionChanged;
        }

        private void TileDayPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Days")
            {
                EvaluateTile((TileViewModel)sender);
            }
        }

        private void EvaluateTile(TileViewModel tile)
        {
            var currentDay = _userData.Configuration.CurrentDay;
            if (tile.Days[currentDay])
            {
                if(!TileViews.Any(v=>v.DataContext == tile))
                {
                    var tileView = tile.CreateTileView();
                    TileViews.Add(tileView);
                }
            }
            else
            {
                if (TileViews.Any(v => v.DataContext == tile))
                {
                    var view = TileViews.First(v => v.DataContext == tile);
                    TileViews.Remove(view);
                }
            }
        }

        private void Exit()
        {
            _userData.Save();
            App.Current.Shutdown(0);
        }

        private void OpenSettingsWindow()
        {
            Configuration.SettingsWindowVisibility = Visibility.Visible;
        }

        private void InitializeCommands()
        {
            OpenSettingsWindowCommand = new DelegateCommand(OpenSettingsWindow);
            ExitCommand = new DelegateCommand(Exit);
        }

        private void TilesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TileViewModel tile in e.NewItems)
                {
                    tile.PropertyChanged += TileDayPropertyChanged;
                    EvaluateTile(tile);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TileViewModel tile in e.OldItems)
                {
                    if(TileViews.Any(v=>v.DataContext == tile))
                    {
                        var view = TileViews.First(v => v.DataContext == tile);
                        TileViews.Remove(view);
                    }
                    tile.PropertyChanged -= TileDayPropertyChanged;
                }
            }
        }
    }
}
