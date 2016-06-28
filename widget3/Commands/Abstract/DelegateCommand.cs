using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace widget3.Commands.Abstract
{
    public class DelegateCommand : ICommand
    {
        private Action _action;

        private Func<bool> _check;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public DelegateCommand(Action action, Func<bool> actionCanExecute)
        {
            _action = action;
            _check = actionCanExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_check == null)
            {
                return true;
            }

            return _check();
        }

        public void Execute(object parameter)
        {
            if (_action == null)
            {
                throw new ArgumentNullException("action");
            }

            _action();
        }

        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                App.Current.Dispatcher.BeginInvoke((Action)(() => CanExecuteChanged(this, EventArgs.Empty)));
            }
        }
    }
}
