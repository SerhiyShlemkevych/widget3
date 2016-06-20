using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.MainWindow;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Concrete.Common
{
    public class TaskTileViewModel : TileViewModel
    {
        public TaskTileViewModel(IUserDataService userData) : base(userData)
        {

        }

        public override TileBase CreateTileView()
        {
            var tileView = new TaskTile();
            SetBindings(tileView);

            return tileView;
        }
    }
}
