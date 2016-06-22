using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Concrete.Common;

namespace widget3.ViewModels.Abstract.SettingsWindow
{
    public abstract class ChildViewModel : ViewModel
    {
        public ChildViewModel(ParentViewModel parent, IUserDataService userData)
        {
            UserData = userData;
            Parent = parent;
        }

        public ConfigurationViewModel Configuration
        {
            get
            {
                return UserData.Configuration;
            }
        }

        protected ParentViewModel Parent
        {
            get;
            private set;
        }

        protected IUserDataService UserData
        {
            get;
            private set;
        }

        public Page Page
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public virtual void Deactivate()
        {

        }

        public virtual void Activate()
        {

        }

        protected virtual void InitializeCommands()
        {
            
        }
    }
}
