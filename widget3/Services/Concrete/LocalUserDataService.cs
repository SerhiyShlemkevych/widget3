using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private T Deserialize<T>(string serializedData, Type[] types)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T), types);
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

        private string SerializeTiles(IEnumerable<TileModel> tiles)
        {
            HashSet<Type> dataTypes = new HashSet<Type>();
            foreach(var tile in tiles)
            {
                dataTypes.Add(tile.Data.GetType());
            }
            string serializedData = string.Empty;
            XmlSerializer serializer = new XmlSerializer(tiles.GetType(), dataTypes.ToArray());
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, tiles);
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
            var path = _basePath + "tileDataTypes.xml";
            if (!File.Exists(path))
            {
                return new TileModel[0];
            }

            Type[] tileDataTypes = null;

            using (StreamReader file = new StreamReader(path))
            {
                var rawData = file.ReadToEnd();
                tileDataTypes = Deserialize<string[]>(rawData).Select(t=>Type.GetType(t)).ToArray();

                if (tileDataTypes == null)
                {
                    tileDataTypes = new Type[0];
                }
            }

            path = _basePath + "tiles.xml";
            using (StreamReader file = new StreamReader(path))
            {
                var rawData = file.ReadToEnd();
                var result = Deserialize<List<TileModel>>(rawData, tileDataTypes);

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
                string serializedData = SerializeTiles(tiles);
                file.Write(serializedData);
            }

            path = _basePath + "tileDataTypes.xml";
            HashSet<Type> tileTypes = new HashSet<Type>();
            foreach (var tile in tiles)
            {
                tileTypes.Add(tile.Data.GetType());
            }
            using (StreamWriter file = new StreamWriter(path))
            {
                string serializedData = Serialize(tileTypes.Select(t=>t.FullName).ToArray());
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

            Type mapper = _mapper.GetType();
            MethodInfo map = mapper.GetMethods().Where(m => m.Name == "Map" && m.GetParameters().Count() == 2).First();

            var tileModels = LoadTileData();
            foreach (var tileModel in tileModels)
            {
                string typeString = String.Format("{0}{1}{2}", "widget3.ViewModels.Concrete.Common.", tileModel.Type.ToString(), "TileViewModel");
                Type type = Type.GetType(typeString);
                var resolvedMap = map.MakeGenericMethod(typeof(TileModel), type);
                var tile = (TileViewModel)resolvedMap.Invoke(_mapper, new object[] { tileModel, new Action<TileModel, TileViewModel>(MapBackground) });
                tile.UserData = this;
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
                var tileModel = _mapper.Map<TileViewModel, TileModel>(tile, MapBackBackground);
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

        private void MapBackground(TileModel model, TileViewModel viewModel)
        {
            viewModel.Background = _mapper.Map<BackgroundModel, BackgroundViewModel>(model.Background);
        }

        private void MapBackBackground(TileViewModel viewModel, TileModel model)
        {
            model.Background = _mapper.Map<BackgroundViewModel, BackgroundModel>(viewModel.Background);
        }
    }
}
