using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using widget3.Code;
using widget3.Converters;
using widget3.Models;
using widget3.Services.Abstract;
using widget3.Services.Concrete;
using widget3.ViewModels.Concrete.MainWindow;
using widget3.ViewModels.Concrete.SettingsWindow;

namespace widget3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindowViewModel _mainWindowViewModel;
        private SettingsWindowViewModel _settingsWindowViewModel;
        private List<ITileDataProvider> _tileDataProviders;

        public App()
        {
            IMapperService mapper = new ReflectionMapper();
            IUserDataService userData = new LocalUserDataService(mapper);
            userData.Load();

            _mainWindowViewModel = new MainWindowViewModel(userData);
            _settingsWindowViewModel = new SettingsWindowViewModel(userData);
            InitializeTileDataProviders(userData);
        }

        private void InitializeTileDataProviders(IUserDataService userData)
        {
            _tileDataProviders = new List<ITileDataProvider>();

            _tileDataProviders.Add(new TaskTileDataProvider(userData));
            _tileDataProviders.Add(new AlarmTileDataProvider(userData));
            _tileDataProviders.Add(new WeatherTileDataProvider(userData));
        }
    }
}
