using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using widget3.Code;
using widget3.Commands.Abstract;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.MainWindow;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class TaskTileViewModel : TileViewModel
    {
        private Visibility _doneVisibility = Visibility.Hidden;

        public TaskTileViewModel()
        {
            Type = Enums.TileType.Task;
        }

        public Visibility DoneVisibility
        {
            get
            {
                return _doneVisibility;
            }
            set
            {
                _doneVisibility = value;
                OnPropertyChanged("DoneVisibility");
            }
        }

        public override TileBase CreateTileView()
        {
            var tileView = new TaskTile() { DataContext = this };
            SetBindings(tileView);
            return tileView;
        }

        public override IEnumerable<CreateTileStep> GetCreateSteps()
        {
            return new List<CreateTileStep>()
            {
                CommonCreateTileSteps.BackgroundStep,
                CommonCreateTileSteps.TextStep,
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
                CommonEditPropertyInfos.Text
            };
        }

        public override void SetDefaultCommand()
        {
            Command = new DelegateCommand(ToggleDone);
        }

        private void ToggleDone()
        {
            if(DoneVisibility == Visibility.Hidden)
            {
                DoneVisibility = Visibility.Visible;
            }
            else
            {
                DoneVisibility = Visibility.Hidden;
            }
        }
    }
}
