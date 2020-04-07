using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    using System;
    using System.Windows.Input;

    namespace ProgressThread
    {
        public class RelayCommand<T> : ICommand
        {
            private readonly Predicate<T> _canExecute;
            private readonly Action<T> _execute;

            public RelayCommand(Action<T> execute)
                : this(execute, null)
            {
                _execute = execute;
            }

            public RelayCommand(Action<T> execute, Predicate<T> canExecute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }
                _execute = execute;
                _canExecute = canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }
        }

        public class RelayCommand : ICommand
        {
            private readonly Predicate<object> _canExecute;
            private readonly Action<object> _execute;

            public RelayCommand(Action<object> execute)
                : this(execute, null)
            {
                _execute = execute;
            }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }
                _execute = execute;
                _canExecute = canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }


        }
    }
}
