using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using widget3.ViewModels.Abstract.Common;

namespace widget3.ViewModels.Abstract.SettingsWindow
{
    public abstract class ParentViewModel : ViewModel
    {
        private ChildViewModel _activeViewModel;

        public ChildViewModel ActiveViewModel
        {
            get
            {
                return _activeViewModel;
            }
            private set
            {
                _activeViewModel = value;
                OnPropertyChanged("ActiveViewModel");
            }
        }

        public void ActivateViewModel(ChildViewModel viewModel)
        {
            ActiveViewModel?.Deactivate();
            ActiveViewModel = viewModel;
            ActiveViewModel?.Activate();
        }
    }
}
