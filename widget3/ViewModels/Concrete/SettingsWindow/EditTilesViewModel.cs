using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using widget3.Commands.Abstract;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Abstract.SettingsWindow;
using widget3.ViewModels.Concrete.Common;
using widget3.Views.Settings;

namespace widget3.ViewModels.Concrete.SettingsWindow
{
    public class EditTilesViewModel : ChildViewModel
    {
        private TileViewModel _selectedTile;
        private CreateTileViewModel _createTileViewModel;

        public EditTilesViewModel(ParentViewModel parent, IUserDataService userData) : base(parent, userData)
        {
            Name = "Edit tiles";
            _createTileViewModel = new CreateTileViewModel(parent, userData, this);
            EditInfoViews = new ObservableCollection<Grid>();
            Days = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            userData.Configuration.CurrentDayChanged += ConfigurationCurrentDayChanged;
            InitializeCommands();
            Page = new EditTilesPage() { DataContext = this };
        }

        private void ConfigurationCurrentDayChanged(object arg1, EventArgs arg2)
        {
            OnPropertyChanged("SelectedDay");
        }

        public ObservableCollection<BackgroundViewModel> Backgrounds
        {
            get
            {
                return UserData.Backgrounds;
            }
        }

        public ObservableCollection<Grid> EditInfoViews
        {
            get;
            private set;
        }

        public Visibility HintVisibility
        {
            get
            {
                return SelectedTile == null ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public DayOfWeek SelectedDay
        {
            get
            {
                return (DayOfWeek)UserData.Configuration.CurrentDay;
            }
            set
            {
                SelectTile(null);
                UserData.Configuration.ChangeCurrentDay((int)value);
            }
        }

        public TileViewModel SelectedTile
        {
            get
            {
                return _selectedTile;
            }
            set
            {
                _selectedTile = value;
                OnPropertyChanged("SelectedTile");
            }
        }

        public List<DayOfWeek> Days
        {
            get;
            private set;
        }

        public DelegateCommand<TileViewModel> SelectTileCommand
        {
            get;
            private set;
        }

        public DelegateCommand DeleteTileCommand
        {
            get;
            private set;
        }

        public DelegateCommand CreateTileCommand
        {
            get;
            private set;
        }

        public DelegateCommand MoveUpCommand
        {
            get;
            private set;
        }

        public DelegateCommand MoveDownCommand
        {
            get;
            private set;
        }

        public DelegateCommand MoveLeftCommand
        {
            get;
            private set;
        }

        public DelegateCommand MoveRightCommand
        {
            get;
            private set;
        }

        private void SelectTile(TileViewModel tile)
        {
            if (tile == SelectedTile)
            {
                tile = null;
            }

            SelectedTile?.Deselect();
            SelectedTile = tile;
            SelectedTile?.Select();
            MoveDownCommand.OnCanExecuteChanged();
            MoveLeftCommand.OnCanExecuteChanged();
            MoveUpCommand.OnCanExecuteChanged();
            MoveRightCommand.OnCanExecuteChanged();
            DeleteTileCommand.OnCanExecuteChanged();
            OnPropertyChanged("HintVisibility");
            if (SelectedTile != null)
            {
                ShowEditInfo();
            }
            else
            {
                HideEditInfo();
            }
        }

        private void HideEditInfo()
        {
            EditInfoViews.Clear();
        }

        private void ShowEditInfo()
        {
            EditInfoViews.Clear();
            var editInfos = SelectedTile.GetEditInfo();
            foreach (var editInfo in editInfos)
            {
                var label = new Label() { Content = editInfo.Label, FontSize = 20, VerticalAlignment = VerticalAlignment.Center };
                label.SetResourceReference(Label.StyleProperty, "LabelStyle");
                editInfo.Control.Margin = new Thickness(5);

                var grid = new Grid();
                var width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = width });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = width });
                Grid.SetColumn(editInfo.Control, 1);
                grid.Children.Add(label);
                grid.Children.Add(editInfo.Control);

                EditInfoViews.Add(grid);
            }
        }

        private void MoveLeft()
        {
            SelectedTile.Column--;
            MoveRightCommand.OnCanExecuteChanged();
            MoveLeftCommand.OnCanExecuteChanged();
        }

        private bool MoveLeftCanExecute()
        {
            return IsAnyTileSelected() && SelectedTile.Column > 0;
        }

        private void MoverRight()
        {
            SelectedTile.Column++;
            MoveLeftCommand.OnCanExecuteChanged();
            MoveRightCommand.OnCanExecuteChanged();
        }

        private bool MoveRightCanExecute()
        {
            return IsAnyTileSelected() && SelectedTile.Column + SelectedTile.Width < UserData.Configuration.ColumnCount;
        }

        private void MoverUp()
        {
            SelectedTile.Row--;
            MoveDownCommand.OnCanExecuteChanged();
            MoveUpCommand.OnCanExecuteChanged();
        }

        private bool MoveUpCanExecute()
        {
            return IsAnyTileSelected() && SelectedTile.Row > 0;
        }

        private void MoveDown()
        {
            SelectedTile.Row++;
            MoveDownCommand.OnCanExecuteChanged();
            MoveUpCommand.OnCanExecuteChanged();
        }

        private bool MoveDownCanExecute()
        {
            return IsAnyTileSelected() && SelectedTile.Row + SelectedTile.Height < UserData.Configuration.RowCount;
        }

        private bool IsAnyTileSelected()
        {
            return SelectedTile != null;
        }

        private void DeleteTile()
        {
            UserData.Tiles.Remove(SelectedTile);
            SelectTile(null);
        }

        private void CreateTile()
        {
            Parent.ActivateViewModel(_createTileViewModel);
        }

        public override void Activate()
        {
            base.Activate();
            foreach (var tile in UserData.Tiles)
            {
                tile.Command = SelectTileCommand;
                tile.CommandParameter = tile;
            }
            foreach (var background in Backgrounds)
            {
                background.Command = null;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            foreach (var tile in UserData.Tiles)
            {
                tile.SetDefaultCommand();
            }
            SelectTile(null);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();
            SelectTileCommand = new DelegateCommand<TileViewModel>(SelectTile);
            MoveDownCommand = new DelegateCommand(MoveDown, MoveDownCanExecute);
            MoveUpCommand = new DelegateCommand(MoverUp, MoveUpCanExecute);
            MoveLeftCommand = new DelegateCommand(MoveLeft, MoveLeftCanExecute);
            MoveRightCommand = new DelegateCommand(MoverRight, MoveRightCanExecute);
            DeleteTileCommand = new DelegateCommand(DeleteTile, IsAnyTileSelected);
            CreateTileCommand = new DelegateCommand(CreateTile);
        }
    }
}
