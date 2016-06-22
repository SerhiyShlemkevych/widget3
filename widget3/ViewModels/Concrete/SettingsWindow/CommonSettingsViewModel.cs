using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.SettingsWindow;
using widget3.ViewModels.Concrete.Common;
using widget3.Views.Settings;

namespace widget3.ViewModels.Concrete.SettingsWindow
{
    public class CommonSettingsViewModel : ChildViewModel
    {
        public CommonSettingsViewModel(ParentViewModel parent, IUserDataService userData) : base(parent, userData)
        {
            Name = "Common settings";
            Page = new CommonSettingsPage() { DataContext = this };
        }
    }
}
