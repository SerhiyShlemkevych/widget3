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

namespace widget3.ViewModels.Concrete.MainWindow
{
    public class MainWindowViewModel : ViewModel
    {
        private IUserDataService _userData;

        private int _rowCount;
        private int _columnCount;
        private int _windowWidth;
        private int _windowHeight;
        private Brush _windowBackground;
        private widget3.Views.Main.MainWindow _window;

        public MainWindowViewModel(IUserDataService userData)
        {
            _userData = userData;
            Tiles = userData.Tiles;
            TileViews = new ObservableCollection<TileBase>();
            InitializeTiles();


            WindowHeight = 800;
            WindowWidth = 1000;
            WindowBackground = Brushes.BlueViolet;

            _window = new Views.Main.MainWindow() { DataContext = this };
            _window.Show();
        }

        public int RowCount
        {
            get
            {
                return _rowCount;
            }
            set
            {
                _rowCount = value;
                OnPropertyChanged("RowCount");
            }
        }

        public int ColumnCount
        {
            get
            {
                return _columnCount;
            }
            set
            {
                _columnCount = value;
                OnPropertyChanged("ColumnCount");
            }
        }

        public int WindowWidth
        {
            get
            {
                return _windowWidth;
            }
            set
            {
                _windowWidth = value;
                OnPropertyChanged("WindowWidth");
            }
        }

        public int WindowHeight
        {
            get
            {
                return _windowHeight;
            }
            set
            {
                _windowHeight = value;
                OnPropertyChanged("WindowHeight");
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

        public ObservableCollection<TileViewModel> Tiles
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
            foreach (var tile in Tiles)
            {
                var tileView = tile.CreateTileView();
                TileViews.Add(tileView);
            }

            Tiles.CollectionChanged += TilesCollectionChanged;
        }

        private void TilesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TileViewModel tile in e.NewItems)
                {
                    var view = tile.CreateTileView();
                    TileViews.Add(view);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TileViewModel tile in e.OldItems)
                {
                    var view = TileViews.First(v => v.DataContext == tile);
                    TileViews.Remove(view);
                }
            }
        }
    }
}
