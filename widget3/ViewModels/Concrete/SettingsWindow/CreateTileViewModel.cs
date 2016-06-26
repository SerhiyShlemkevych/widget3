using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using widget3.Code;
using widget3.Commands.Abstract;
using widget3.Controls.Abstract.Common;
using widget3.Enums;
using widget3.Services.Abstract;
using widget3.ViewModels.Abstract.Common;
using widget3.ViewModels.Abstract.SettingsWindow;
using widget3.ViewModels.Concrete.Common;
using widget3.Views.Settings;
using widget3.Views.Settings.CreateTilePages;

namespace widget3.ViewModels.Concrete.SettingsWindow
{
    public class CreateTileViewModel : ChildViewModel
    {
        private TileViewModel _tile;
        private TileType _selectedType;
        private string _leftButtonContent;
        private string _rightButtonContent;
        private DelegateCommand _rightButtonCommand;
        private DelegateCommand _leftButtonCommand;
        private List<CreateTileStep> _createSteps;
        private int _currentStepIndex;

        private ChildViewModel _previousViewModel;

        public CreateTileViewModel(ParentViewModel parent, IUserDataService userData, ChildViewModel previousViewModel) : base(parent, userData)
        {
            InitializeCommands();
            _previousViewModel = previousViewModel;
            BackgroundViews = new ObservableCollection<TileBase>();
            TileTypes = new List<TileType>() { TileType.Alarm, TileType.Task, TileType.Timer, TileType.Weather, TileType.DateTime };
            _createSteps = new List<CreateTileStep>();
            Page = new CteateTilePage() { DataContext = this };
        }

       

        public ObservableCollection<TileBase> BackgroundViews
        {
            get;
            private set;
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

        public TileType SelectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                OnPropertyChanged("SelectedType");
                OnTileTypeChanged();
            }
        }

        public List<TileType> TileTypes
        {
            get;
            private set;
        }

        public DelegateCommand NextPageCommand
        {
            get;
            private set;
        }
        public DelegateCommand PreviousPageCommand
        {
            get;
            private set;
        }
        public DelegateCommand FinishCommand
        {
            get;
            private set;
        }
        public DelegateCommand CancelCommand
        {
            get;
            private set;
        }

        public TileViewModel Tile
        {
            get
            {
                return _tile;
            }
            set
            {
                _tile = value;
                OnPropertyChanged("Tile");
            }
        }

        public CreateTileStep CurrentStep
        {
            get
            {
                return _createSteps[_currentStepIndex];
            }
        }

        public DelegateCommand RightButtonCommand
        {
            get
            {
                return _rightButtonCommand;
            }
            set
            {
                _rightButtonCommand = value;
                OnPropertyChanged("RightButtonCommand");
            }
        }
        public DelegateCommand LeftButtonCommand
        {
            get
            {
                return _leftButtonCommand;
            }
            set
            {
                _leftButtonCommand = value;
                OnPropertyChanged("LeftButtonCommand");
            }
        }
        public string LeftButtonContent
        {
            get
            {
                return _leftButtonContent;
            }
            set
            {
                _leftButtonContent = value;
                OnPropertyChanged("LeftButtonContent");
            }
        }
        public string RightButtonContent
        {
            get
            {
                return _rightButtonContent;
            }
            set
            {
                _rightButtonContent = value;
                OnPropertyChanged("RightButtonContent");
            }
        }

        public override void Activate()
        {
            base.Activate();
            SelectedType = 0;
            InitializeBackgroundViews();
            _currentStepIndex = 0;
            SetSteps(new List<CreateTileStep>());
            OnPropertyChanged("CurrentStep");
        }

        public override void Deactivate()
        {
            base.Deactivate();
            SelectedType = 0;
            _tile = null;
        }

        protected override void InitializeCommands()
        {
            NextPageCommand = new DelegateCommand(NextPage, NextPageCanExecute);
            PreviousPageCommand = new DelegateCommand(PreviousPage);
            CancelCommand = new DelegateCommand(Cancel);
            FinishCommand = new DelegateCommand(Finish, NextPageCanExecute);
            SelectBackgroundCommand = new DelegateCommand<BackgroundViewModel>(SelectBackground);
        }

        public DelegateCommand<BackgroundViewModel> SelectBackgroundCommand
        {
            get;
            private set;
        }

        private void OnTileTypeChanged()
        {
            if(SelectedType == 0)
            {
                return;
            }

            string typeString = String.Format("{0}{1}{2}", "widget3.ViewModels.Concrete.Common.", SelectedType.ToString(), "TileViewModel");
            Type type = Type.GetType(typeString);
            Tile = (TileViewModel)Activator.CreateInstance(type);
            Tile.UserData = UserData;
            MethodInfo getWizardPagesMethod = type.GetMethod("GetCreateSteps");
            var createSteps = (IEnumerable<CreateTileStep>)getWizardPagesMethod.Invoke(Tile, null);
            foreach (var step in createSteps)
            {
                if (step.PageViewModel == null)
                {
                    step.Page.DataContext = this;
                }
                else
                {
                    var viewModel = Activator.CreateInstance(step.PageViewModel, Tile);
                    step.Page.DataContext = viewModel;
                }
            }
            SetSteps(createSteps);
            Tile.PropertyChanged += TilePropertyChanged;
        }

        private void TilePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NextPageCommand?.OnCanExecuteChanged();
            FinishCommand?.OnCanExecuteChanged();
        }

        private void SetSteps(IEnumerable<CreateTileStep> steps)
        {
            var typeStep = new CreateTileStep()
            {
                Page = new SelectTypePage() { DataContext = this },
                PageHeader = "Select tile type",
                Validate = (tile) => tile != null && tile.Type != 0
            };
            _createSteps.Clear();
            _createSteps.Add(typeStep);
            _createSteps.AddRange(steps);
            LeftButtonCommand = CancelCommand;           
            LeftButtonContent = "Cancel";
            
            if(_createSteps.Count == 1)
            {
                RightButtonCommand = FinishCommand;
                RightButtonContent = "Finish";
            }
            else
            {
                RightButtonCommand = NextPageCommand;
                RightButtonContent = "Next";
            }
            NextPageCommand.OnCanExecuteChanged();
            FinishCommand.OnCanExecuteChanged();
        }

        private void SelectBackground(BackgroundViewModel background)
        {
            Tile.Background?.Deselect();
            Tile.Background = background;
            Tile.Background?.Select();
        }

        private void NextPage()
        {
            _currentStepIndex++;
            if (_currentStepIndex == _createSteps.Count - 1)
            {
                RightButtonCommand = FinishCommand;
                RightButtonContent = "Finish";
            }
            else
            {
                RightButtonCommand = NextPageCommand;
                RightButtonContent = "Next";
            }
            if (_currentStepIndex == 0)
            {
                LeftButtonCommand = CancelCommand;
                LeftButtonContent = "Cancel";
            }
            else
            {
                LeftButtonCommand = PreviousPageCommand;
                LeftButtonContent = "Back";
            }
            OnPropertyChanged("CurrentStep");
            NextPageCommand.OnCanExecuteChanged();
            FinishCommand.OnCanExecuteChanged();
        }

        private bool NextPageCanExecute()
        {
            return CurrentStep.Validate(Tile);
        }

        private void PreviousPage()
        {
            _currentStepIndex--;
            if (_currentStepIndex == _createSteps.Count - 1)
            {
                RightButtonCommand = FinishCommand;
                RightButtonContent = "Finish";
            }
            else
            {
                RightButtonCommand = NextPageCommand;
                RightButtonContent = "Next";
            }
            if (_currentStepIndex == 0)
            {
                LeftButtonCommand = CancelCommand;
                LeftButtonContent = "Cancel";
            }
            else
            {
                LeftButtonCommand = PreviousPageCommand;
                LeftButtonContent = "Back";
            }
            OnPropertyChanged("CurrentStep");
            NextPageCommand.OnCanExecuteChanged();
            FinishCommand.OnCanExecuteChanged();
        }

        private void Finish()
        {
            UserData.Tiles.Add(Tile);
            Parent.ActivateViewModel(_previousViewModel);
        }

        private void Cancel()
        {
            Parent.ActivateViewModel(_previousViewModel);
        }

    }
}
