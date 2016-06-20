using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using widget3.Models;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Concrete.Common;

namespace widget3.Services.Concrete
{
    class LocalUserDataService : IUserDataService
    {
        private string _basePath = "";

        private IMapperService _mapper;

        private ConfigurationViewModel _configuration;
        private ObservableCollection<TileViewModel> _tiles;
        private ObservableCollection<BackgroundViewModel> _backgrounds;

        public LocalUserDataService(IMapperService mapper)
        {
            _mapper = mapper;
            _configuration = new ConfigurationViewModel();
            _tiles = new ObservableCollection<TileViewModel>();
            _backgrounds = new ObservableCollection<BackgroundViewModel>();
        }

        public ObservableCollection<TileViewModel> Tiles
        {
            get
            {
                return _tiles;
            }
        }

        public ConfigurationViewModel Configuration
        {
            get
            {
                return _configuration;
            }
        }

        public ObservableCollection<BackgroundViewModel> Backgrounds
        {
            get
            {
                return _backgrounds;
            }
        }

        private T Deserialize<T>(string serializedData)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(serializedData))
            {
                return (T)deserializer.Deserialize(tr);
            }
        }

        private string Serialize(object obj)
        {
            string serializedData = string.Empty;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, obj);
                serializedData = sw.ToString();
            }
            return serializedData;
        }

        private IEnumerable<BackgroundModel> LoadBackgrounds()
        {
            var path = _basePath + "backgrounds.xml";
            if (!File.Exists(path))
            {
                return new BackgroundModel[0];
            }
            using(StreamReader file = new StreamReader(path))
            {
                var rawData = file.ReadToEnd();
                var result = Deserialize<List<BackgroundModel>>(rawData);

                if(result == null)
                {
                    return new BackgroundModel[0];
                }

                return result;
            }
        }

        private ConfigurationModel LoadConfiguration()
        {
            var path = _basePath + "configuration.xml";
            if (!File.Exists(path))
            {
                return GetDefaultConfiguration();
            }
            using (StreamReader file = new StreamReader(path))
            {
                var rawData = file.ReadToEnd();
                var result = Deserialize<ConfigurationModel>(rawData);

                if (result == null)
                {
                    return GetDefaultConfiguration();
                }

                return result;
            }
        }

        private IEnumerable<TileModel> LoadTileData()
        {
            var path = _basePath + "tiles.xml";
            if (!File.Exists(path))
            {
                return new TileModel[0];
            }
            using (StreamReader file = new StreamReader(path))
            {
                var rawData = file.ReadToEnd();
                var result = Deserialize<List<TileModel>>(rawData);

                if (result == null)
                {
                    return new TileModel[0];
                }

                return result;
            }
        }

        private void SaveBackgrounds(IEnumerable<BackgroundModel> backgrounds)
        {
            var path = _basePath + "backgrounds.xml";
            using(StreamWriter file = new StreamWriter(path))
            {
                string serializedData = Serialize(backgrounds);
                file.Write(serializedData);
            }
        }

        private void SaveConfiguration(ConfigurationModel configuration)
        {
            var path = _basePath + "configuration.xml";
            using (StreamWriter file = new StreamWriter(path))
            {
                string serializedData = Serialize(configuration);
                file.Write(serializedData);
            }
        }

        private void SaveTileData(IEnumerable<TileModel> tiles)
        {
            var path = _basePath + "tiles.xml";
            using (StreamWriter file = new StreamWriter(path))
            {
                string serializedData = Serialize(tiles);
                file.Write(serializedData);
            }
        }

        private ConfigurationModel GetDefaultConfiguration()
        {
            return new ConfigurationModel()
            {
                TileSize = 100,
                TileMargin = 5,
                FontSize = 20,
                SubFontSize = 10
            };
        }

        public void Load()
        {
            var configutarionModel = LoadConfiguration();
            _configuration = _mapper.Map<ConfigurationModel, ConfigurationViewModel>(configutarionModel);

            var tileModels = LoadTileData();
            foreach (var tileModel in tileModels)
            {
                var tile = _mapper.Map<TileModel, TileViewModel>(tileModel);
                Tiles.Add(tile);
            }

            var backgroundModels = LoadBackgrounds();
            foreach(var backgroundModel in backgroundModels)
            {
                var background = _mapper.Map<BackgroundModel, BackgroundViewModel>(backgroundModel);
                Backgrounds.Add(background);
            }
        }

        public void Save()
        {
            var configutarionModel = _mapper.Map<ConfigurationViewModel, ConfigurationModel>(Configuration);
            SaveConfiguration(configutarionModel);

            var tileModels = new List<TileModel>();
            foreach (var tile in Tiles)
            {
                var tileModel = _mapper.Map<TileViewModel, TileModel>(tile);
                tileModels.Add(tileModel);
            }
            SaveTileData(tileModels);

            var backgroundModels = new List<BackgroundModel>();
            foreach (var background in Backgrounds)
            {
                var backgroundModel = _mapper.Map<BackgroundViewModel, BackgroundModel>(background);
                backgroundModels.Add(backgroundModel);
            }
            SaveBackgrounds(backgroundModels);
        }
    }
}
