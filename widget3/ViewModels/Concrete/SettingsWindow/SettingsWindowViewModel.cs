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
        private Visibility _windowVisibility = Visibility.Hidden;
        private Window _window;

        public SettingsWindowViewModel(IUserDataService userData)
        {
            InitializeCommands();
            ChildViewModels = new List<ChildViewModel>();
            ChildViewModelButtons = new ObservableCollection<Button>();
            _userData = userData;
            RegisterChildViewModels();

            _window = new widget3.Views.Settings.SettingsWindow() { DataContext = this, Visibility = Visibility.Hidden, WindowStartupLocation = WindowStartupLocation.Manual, Top = -2000, Left = -2000 };
            _window.Show();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                _window.Dispatcher.Invoke(() => _window.Top = 100);
                _window.Dispatcher.Invoke(() => _window.Left = 100);
            });
        }

        public DelegateCommand ShowWindowCommand
        {
            get;
            private set;
        }

        public DelegateCommand ExitCommand
        {
            get;
            private set;
        }

        public Visibility WindowVisibility
        {
            get
            {
                return _windowVisibility;
            }
            set
            {
                _windowVisibility = value;
                OnPropertyChanged("WindowVisibility");
                if (value == Visibility.Hidden)
                {
                    ActiveViewModel?.Deactivate();
                    _userData.Save();
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

        private void ShowWindow()
        {
            WindowVisibility = Visibility.Visible;
        }

        private void Exit()
        {
            _userData.Save();
            App.Current.Shutdown(0);
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

        private void InitializeCommands()
        {
            _activateViewModelCommand = new DelegateCommand<ChildViewModel>(ActivateViewModel);
            ShowWindowCommand = new DelegateCommand(ShowWindow);
            ExitCommand = new DelegateCommand(Exit);
        }

        private void RegisterChildViewModels()
        {
            RegisterChildViewModel<CommonSettingsViewModel>();
            RegisterChildViewModel<EditTilesViewModel>();
            RegisterChildViewModel<FavoriteBackgroundsViewModel>();
        }
    }
}
