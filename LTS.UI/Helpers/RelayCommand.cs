using System;
using System.Windows.Input;

namespace LTS.UI.Helpers
{
    public class RelayCommand :
        ICommand
    {
        private readonly Action<object?>?
            _executeWithParam;

        private readonly Action?
            _execute;

        private readonly Func<object?, bool>?
            _canExecute;

        // WITHOUT PARAMETER

        public RelayCommand(
            Action execute)
        {
            _execute = execute;
        }

        // WITH PARAMETER

        public RelayCommand(
            Action<object?> execute,
            Func<object?, bool>? canExecute = null)
        {
            _executeWithParam = execute;

            _canExecute = canExecute;
        }

        public event EventHandler?
            CanExecuteChanged;

        public bool CanExecute(
            object? parameter)
        {
            return _canExecute == null
                || _canExecute(parameter);
        }

        public void Execute(
            object? parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
            else
            {
                _executeWithParam?.Invoke(
                    parameter);
            }
        }
    }
}