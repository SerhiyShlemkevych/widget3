using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using widget3.Commands.Abstract;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.SettingsWindow;
using widget3.ViewModels.Concrete.Common;

namespace widget3.ViewModels.Concrete.SettingsWindow
{
    class SettingsWindowViewModel : ParentViewModel
    {
        private IUserDataService _userData;
        private DelegateCommand<ChildViewModel> _activateViewModelCommand;

        private Window _window;

        public SettingsWindowViewModel(IUserDataService userData)
        {
            _activateViewModelCommand = new DelegateCommand<ChildViewModel>(ActivateViewModel);
            ChildViewModels = new List<ChildViewModel>();
            ChildViewModelButtons = new ObservableCollection<Button>();
            _userData = userData;
            _userData.Configuration.PropertyChanged += ConfigurationSettingWindowVisibilityPropertyChanged;
            RegisterChildViewModels();

            _window = new widget3.Views.Settings.SettingsWindow() { DataContext = this, Visibility = Visibility.Hidden, WindowStartupLocation = WindowStartupLocation.Manual, Top = -2000, Left=-2000 };
            _window.Show();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                _window.Dispatcher.Invoke(() => _window.Top = 100);
                _window.Dispatcher.Invoke(() => _window.Left = 100);
            });
        }

        private void ConfigurationSettingWindowVisibilityPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "SettingsWindowVisibility")
            {
                if(_userData.Configuration.SettingsWindowVisibility == Visibility.Hidden)
                {
                    ActiveViewModel?.Deactivate();
                }
            }
        }

        public List<ChildViewModel> ChildViewModels
        {
            get;
            private set;
        }

        public ObservableCollection<Button> ChildViewModelButtons
        {
            get;
            private set;
        }

        public ConfigurationViewModel Configuration
        {
            get
            {
                return _userData.Configuration;
            }
        }

        private void RegisterChildViewModel<T>() where T : ChildViewModel
        {
            var viewModel = (T)Activator.CreateInstance(typeof(T), this, _userData);
            ChildViewModels.Add(viewModel);
            var button = new Button()
            {
                Content = viewModel.Name,
                Command = _activateViewModelCommand,
                CommandParameter = viewModel,
                Margin = new Thickness(5)
            };
            button.SetResourceReference(Button.StyleProperty, "ButtonStyle");

            ChildViewModelButtons.Add(button);
        }

        private void RegisterChildViewModels()
        {
            RegisterChildViewModel<CommonSettingsViewModel>();
            RegisterChildViewModel<EditTilesViewModel>();
            RegisterChildViewModel<FavoriteBackgroundsViewModel>();
        }
    }
}
