using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eurofurence.Companion.Common
{
    public class AwaitableCommand: AwaitableCommand<object>
    {
        public AwaitableCommand(Func<Task> executeMethod)
            :base(o=>executeMethod())
        {

        }

    }

    public class AwaitableCommand<T> : ICommand
    {
        private readonly Func<T, Task> _executeMethod;
        private bool _isExecuting;

        public AwaitableCommand(Func<T, Task> executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                await _executeMethod(obj);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public ICommand Command => this;

        public bool CanExecute(object parameter)
        {
            return !_isExecuting;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }        

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
