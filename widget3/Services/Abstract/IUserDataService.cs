using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Models;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Concrete.Common;

namespace widget3.Services.Abstract
{
    public interface IUserDataService
    {
        void Load();

        void Save();

        ObservableCollection<TileViewModel> Tiles
        {
            get;
        }

        ConfigurationViewModel Configuration
        {
            get;
        }

        ObservableCollection<BackgroundViewModel> Backgrounds
        {
            get;
        }
    }
}
