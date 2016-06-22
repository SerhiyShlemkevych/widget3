using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using widget3.ViewModels.Abstract.Common;

namespace widget3.Code
{
    public class CreateTileStep
    {
        public string PageHeader
        {
            get;
            set;
        }

        public Page Page
        {
            get;
            set;
        }

        public Func<TileViewModel, bool> Validate
        {
            get;
            set;
        }
    }
}
