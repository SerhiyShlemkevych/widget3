using Gat.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using widget3.Commands.Abstract;
using widget3.Controls.Abstract.Common;
using widget3.Controls.Concrete.MainWindow;
using widget3.Enums;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.SettingsWindow;
using widget3.ViewModels.Concrete.Common;
using widget3.Views.Settings;

namespace widget3.ViewModels.Concrete.SettingsWindow
{
    public class FavoriteBackgroundsViewModel : ChildViewModel
    {
        private BackgroundViewModel _selectedBackground;
        private Visibility _createViewVisibility = Visibility.Hidden;
        private string _selectedFilePath;
        private Color? _selectedColor;
        private OpenFileDialog _fileDialog;

        public FavoriteBackgroundsViewModel(ParentViewModel parent, IUserDataService userData) : base(parent, userData)
        {
            InitializeCommands();
            BackgroundViews = new ObservableCollection<TileBase>();          
            Name = "Favorite Backgrounds";
            UserData.Backgrounds.CollectionChanged += BackgroundsCollectionChanged;     
            Page = new FavoriteBackgroundsPage() { DataContext = this };
        }

        public DelegateCommand CreateBackgroundColorCommand
        {
            get;
            private set;
        }

        public DelegateCommand CreateBackgroundImageCommand
        {
            get;
            private set;
        }

        public DelegateCommand DeleteBackgroundCommand
        {
            get;
            private set;
        }

        public DelegateCommand OpenFileDialogCommand
        {
            get;
            private set;
        }
        

        public DelegateCommand ShowCreateViewCommand
        {
            get;
            private set;
        }

        public DelegateCommand HideCreateViewCommand
        {
            get;
            private set;
        }
        

        public DelegateCommand<BackgroundViewModel> SelectBackgroundCommand
        {
            get;
            private set;
        }



        public Color? SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                _selectedColor = value;
                CreateBackgroundColorCommand?.OnCanExecuteChanged();
                OnPropertyChanged("SelectedColor");
            }
        }

        public string SelectedFilePath
        {
            get
            {
                return _selectedFilePath;
            }
            set
            {
                _selectedFilePath = value;
                CreateBackgroundImageCommand?.OnCanExecuteChanged();
                OnPropertyChanged("SelectedFilePath");
            }
        }

        public ObservableCollection<TileBase> BackgroundViews
        {
            get;
            private set;
        }

        public Visibility CreateViewVisibility
        {
            get
            {
                return _createViewVisibility;
            }
            set
            {
                _createViewVisibility = value;
                OnPropertyChanged("CreateViewVisibility");
            }
        }

        public BackgroundViewModel SelectedBackground
        {
            get
            {
                return _selectedBackground;
            }
            set
            {
                _selectedBackground = value;
                OnPropertyChanged("SelectedBackground");
            }
        }

        private void OpenFileDialog()
        {
            _fileDialog = new OpenFileDialog();
            _fileDialog.FileOk += _fileDialogFileOk;
            _fileDialog.ShowDialog();                     
        }

        private void _fileDialogFileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SelectedFilePath = _fileDialog.FileName;
        }

        private void HideCreateView()
        {
            CreateViewVisibility = Visibility.Hidden;
        }

        private void SelectBackground(BackgroundViewModel background)
        {
            SelectedBackground?.Deselect();
            SelectedBackground = background;
            SelectedBackground?.Select();
            DeleteBackgroundCommand?.OnCanExecuteChanged();
        }

        private void CreateBackgroundColor()
        {
            var background = new BackgroundViewModel()
            {
                Type = BackgroundType.SolidColor,
                BackgroundData = SelectedColor.ToString()
            };

            UserData.Backgrounds.Add(background);
            HideCreateView();
        }

        private bool CreateBackgroundColorCanExecute()
        {
            return SelectedColor != null;
        }

        private void CreateBackgroundImage()
        {
            var background = new BackgroundViewModel()
            {
                Type = BackgroundType.Image,
                BackgroundData = SelectedFilePath
            };

            UserData.Backgrounds.Add(background);
            HideCreateView();
            SelectedFilePath = null;
        }

        private bool CreateBackgroundImageCanExecute()
        {
            return !string.IsNullOrEmpty(SelectedFilePath);
        }

        private void ShowCreateView()
        {
            CreateViewVisibility = Visibility.Visible;
        }

        private void DeleteBackground()
        {
            UserData.Backgrounds.Remove(SelectedBackground);
        }

        private bool DeleteBackgroundCanExecute()
        {
            return SelectedBackground != null;
        }

        private void InitializeBackgroundView(BackgroundViewModel background)
        {
            var view = background.CreateTileView();
            background.Command = SelectBackgroundCommand;
            background.CommandParameter = background;
            BackgroundViews.Add(view);
        }

        private void InitializeBackgroundViews()
        {
            BackgroundViews.Clear();
            foreach (var background in UserData.Backgrounds)
            {
                InitializeBackgroundView(background);
            }

        }

        private void BackgroundsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BackgroundViewModel background in e.NewItems)
                {
                    var view = background.CreateTileView();
                    background.Command = SelectBackgroundCommand;
                    background.CommandParameter = background;
                    BackgroundViews.Add(view);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BackgroundViewModel background in e.OldItems)
                {
                    var view = BackgroundViews.First(v => v.DataContext == background);
                    BackgroundViews.Remove(view);
                }
            }
        }

        protected override void InitializeCommands()
        {
            ShowCreateViewCommand = new DelegateCommand(ShowCreateView);
            CreateBackgroundColorCommand = new DelegateCommand(CreateBackgroundColor, CreateBackgroundColorCanExecute);
            CreateBackgroundImageCommand = new DelegateCommand(CreateBackgroundImage, CreateBackgroundImageCanExecute);
            DeleteBackgroundCommand = new DelegateCommand(DeleteBackground, DeleteBackgroundCanExecute);
            HideCreateViewCommand = new DelegateCommand(HideCreateView);
            OpenFileDialogCommand = new DelegateCommand(OpenFileDialog);
            SelectBackgroundCommand = new DelegateCommand<BackgroundViewModel>(SelectBackground);
        }

        public override void Activate()
        {
            base.Activate();
            InitializeBackgroundViews();
        }


    }
}
