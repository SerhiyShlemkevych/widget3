using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using widget3.Code;
using widget3.Commands.Abstract;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.MainWindow;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class AlarmTileViewModel : TileViewModel
    {
        private bool _isAlarmDisabled;

        public AlarmTileViewModel()
        {
            Type = Enums.TileType.Alarm;
        }

        public Visibility ActiveVisibility
        {
            get
            {
                return IsAlarmDisabled ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public bool IsAlarmDisabled
        {
            get
            {
                return _isAlarmDisabled;
            }
            set
            {
                _isAlarmDisabled = value;
                OnPropertyChanged("IsAlarmDisabled");
                OnPropertyChanged("ActiveVisibility");
            }
        }

        private void ToggleAlarm()
        {
            IsAlarmDisabled = !IsAlarmDisabled;
        }

        public override TileBase CreateTileView()
        {
            var view = new AlarmTile() { DataContext = this };
            SetBindings(view);
            return view;
        }

        public override IEnumerable<CreateTileStep> GetCreateSteps()
        {
            return new List<CreateTileStep>()
            {
                CommonCreateTileSteps.BackgroundStep,
                CommonCreateTileSteps.TimeStep,
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
                CommonEditPropertyInfos.Time,
                CommonEditPropertyInfos.Days
            };
        }

        public override void SetDefaultCommand()
        {
            Command = new DelegateCommand(ToggleAlarm);
        }

    }
}
