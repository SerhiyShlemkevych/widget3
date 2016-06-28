using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Code;
using widget3.Commands.Abstract;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.MainWindow;
using widget3.Enums;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Concrete.SettingsWindow;
using widget3.Views.Settings.CreateTilePages;

namespace widget3.ViewModels.Concrete.Common
{
    public class WeatherTileViewModel : TileViewModel
    {

        public WeatherTileViewModel()
        {
            Type = TileType.Weather;
        }

        private string _location;

        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        private string _weatherCondition;

        public string WeatherCondition
        {
            get
            {
                return _weatherCondition;
            }
            set
            {
                _weatherCondition = value;
                OnPropertyChanged("WeatherCondition");
            }
        }


        public override TileBase CreateTileView()
        {
            var view = new WeatherTile() { DataContext = this };
            SetBindings(view);
            return view;
        }

        public override IEnumerable<CreateTileStep> GetCreateSteps()
        {
            return new List<CreateTileStep>()
            {
                new CreateTileStep()
                {
                    PageHeader = "Select city",
                    Page = new SelectWeatherPlacePage(),
                    PageViewModel = typeof(WeatherStepViewModel),
                    Validate = (tile)=>tile.Data != null
                },
                CommonCreateTileSteps.BackgroundStep,
                CommonCreateTileSteps.SizeStep,
                CommonCreateTileSteps.DaysStep
            };
        }

        private void Refresh()
        {
            Data = Data;
        }

        public override IEnumerable<TileEditPropertyInfo> GetEditInfo()
        {
            return new List<TileEditPropertyInfo>()
            {
                CommonEditPropertyInfos.Background,
                CommonEditPropertyInfos.Transperency,
                CommonEditPropertyInfos.Height,
                CommonEditPropertyInfos.Width,
                CommonEditPropertyInfos.Days

            };
        }

        public override void SetDefaultCommand()
        {
            Command = new DelegateCommand(Refresh);
        }
    }
}
