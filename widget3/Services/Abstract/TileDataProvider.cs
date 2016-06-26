using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using widget3.ViewModels.Abstract.Common;
using System.ComponentModel;

namespace widget3.Services.Abstract
{
    public abstract class TileDataProvider<TTile> : ITileDataProvider where TTile : TileViewModel
    {
        IUserDataService _userData;

        ObservableCollection<TTile> _tiles;

        public TileDataProvider(IUserDataService userData)
        {
            _userData = userData;
            _tiles = new ObservableCollection<TTile>();
            InitializeExistedTiles();
            userData.Tiles.CollectionChanged += OnTilesChanged;
        }

        private void InitializeExistedTiles()
        {
            foreach(var tile in _userData.Tiles)
            {
                if(tile is TTile)
                {
                    _tiles.Add((TTile)tile);
                    tile.PropertyChanged += TileDataPropertyChanged;
                }
            }
        }

        private void TileDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Data")
            {
                ProvideTileWithValue((TTile)sender);
            }
            OnTilePropertyChanged(sender, e);            
        }

        protected void RefreshAllTiles()
        {
            foreach(var tile in _tiles)
            {
                ProvideTileWithValue(tile);
            }
        }

        protected ObservableCollection<TTile> Tiles
        {
            get
            {
                return _tiles;
            }
        }

        protected virtual void OnTilePropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        protected abstract void ProvideTileWithValue(TTile tile);

        private void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is TTile)
                    {
                        _tiles.Add((TTile)item);
                        ((TTile)item).PropertyChanged += TileDataPropertyChanged;
                        ProvideTileWithValue((TTile)item);
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is TTile)
                    {
                        if (_tiles.Contains((TTile)item))
                        {
                            ((TTile)item).PropertyChanged -= TileDataPropertyChanged;
                            _tiles.Remove((TTile)item);
                        }
                    }
                }
            }
        }
    }
}
