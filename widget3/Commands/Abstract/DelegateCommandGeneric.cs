using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace widget3.Commands.Abstract
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> _action;

        private Func<T, bool> _check;

        public DelegateCommand(Action<T> action)
        {
            _action = action;
        }

        public DelegateCommand(Action<T> action, Func<T, bool> actionCanExecute)
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

            return _check((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (_action == null)
            {
                throw new ArgumentNullException("action");
            }

            _action((T)parameter);
        }

        public void OnCanExecuteChanged()
        {
            if(CanExecuteChanged!= null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
