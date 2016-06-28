using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.ViewModels.Abstract.Common;
using widget3.Views.Settings.CreateTilePages;

namespace widget3.Code
{
    public static class CommonCreateTileSteps
    {
        public static CreateTileStep BackgroundStep
        {
            get
            {
                return new CreateTileStep()
                {
                    PageHeader = "Select background",
                    Page = new SelectBackgroundPage(),
                    Validate = (tile) => tile != null && tile.Background != null
                };
            }
        }

        public static CreateTileStep TextStep
        {
            get
            {
                return new CreateTileStep()
                {
                    PageHeader = "Enter desired text",
                    Page = new SelectTextPage(),
                    Validate = (tile) => tile != null && tile.Data !=null
                };
            }
        }

        public static CreateTileStep TimeStep
        {
            get
            {
                return new CreateTileStep()
                {
                    PageHeader = "Enter time",
                    Page = new SelectTimePage(),
                    Validate = (tile) => tile != null && tile.Data != null
                };
            }
        }

        public static CreateTileStep SizeStep
        {
            get
            {
                return new CreateTileStep()
                {
                    PageHeader = "Select size",
                    Page = new SelectSizePage(),
                    Validate = (tile) => tile != null && tile.Width != 0 && tile.Height != 0
                };
            }
        }

        public static CreateTileStep DaysStep
        {
            get
            {
                return new CreateTileStep()
                {
                    PageHeader = "Select days for tile",
                    Page = new SelectDaysPage(),
                    Validate = (tile) => tile != null && tile.Days.Any(d => d)
                };
            }
        }

    }
}
