using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Code;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.MainWindow;
using widget3.Enums;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class DateTimeTileViewModel : TileViewModel
    {
        private string _date;
        private string _day;

        public DateTimeTileViewModel()
        {
            Type = TileType.DateTime;
        }

        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }


        public string Day
        {
            get
            {
                return _day;
            }
            set
            {
                _day = value;
                OnPropertyChanged("Day");
            }
        }



        public override TileBase CreateTileView()
        {
            var tileView = new DateTimeTile() { DataContext = this };
            SetBindings(tileView);
            return tileView;
        }

        public override IEnumerable<CreateTileStep> GetCreateSteps()
        {
            return new List<CreateTileStep>()
            {
                CommonCreateTileSteps.BackgroundStep,
                CommonCreateTileSteps.SizeStep,
                CommonCreateTileSteps.DaysStep
            };
        }

        public override IEnumerable<TileEditPropertyInfo> GetEditInfo()
        {
            return new List<TileEditPropertyInfo>()
            {
                CommonEditPropertyInfos.Background,
                CommonEditPropertyInfos.Transperency,
                CommonEditPropertyInfos.Width,
                CommonEditPropertyInfos.Height,
                CommonEditPropertyInfos.Days
            };
        }

        public override void SetDefaultCommand()
        {
            
        }
    }
}
