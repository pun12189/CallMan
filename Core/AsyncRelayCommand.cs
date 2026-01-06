using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BahiKitab.Core
{
    public class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private bool _isExecuting;

        public event EventHandler CanExecuteChanged;

        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_isExecuting)
                return false;

            // Handle potential null parameter for generic T if needed
            T genericParameter = parameter == null ? default : (T)parameter;
            return _canExecute?.Invoke(genericParameter) ?? true;
        }

        public async void Execute(object parameter)
        {
            // Use an explicit cast to T for safety
            T genericParameter = parameter == null ? default : (T)parameter;

            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                    await _execute(genericParameter);
                }
                finally
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            // Raises the CanExecuteChanged event, often tied to the CommandManager
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
