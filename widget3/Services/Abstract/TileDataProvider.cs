using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace widget3.Services.Abstract
{
    public abstract class TileDataProvider<TTile>
    {
        ICommonDataService _commonData;

        ObservableCollection<TTile> _tiles;

        public TileDataProvider(ICommonDataService commonData)
        {
            _tiles = new ObservableCollection<TTile>();
            _tiles.CollectionChanged += OnTilesChanged;
            _commonData = commonData;
        }

        protected abstract void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e);

        private void OnTilesChangedCommon(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach(var item in e.NewItems)
            {
                if(item is TTile)
                {
                    _tiles.Add((TTile)item);
                }
            }

            foreach (var item in e.OldItems)
            {
                if (item is TTile)
                {
                    if (_tiles.Contains((TTile)item))
                    {
                        _tiles.Remove((TTile)item);
                    }                    
                }
            }
        }
    }
}
