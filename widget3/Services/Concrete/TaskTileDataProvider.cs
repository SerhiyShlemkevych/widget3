using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Controls.Concrete.MainWindow;
using widget3.Services.Abstract;
using widget3.ViewModels.Concrete.Common;

namespace widget3.Services.Concrete
{
    public class TaskTileDataProvider : TileDataProvider<TaskTileViewModel>
    {
        public TaskTileDataProvider(IUserDataService userData) : base(userData)
        {
        }

        protected override void ProvideTileWithValue(TaskTileViewModel tile)
        {
            tile.Text = (string)tile.Data;
        }
    }
}
