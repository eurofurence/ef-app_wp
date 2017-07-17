using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eurofurence.Companion.Common
{
    public class AwaitableCommand: AwaitableCommand<object>
    {
        public AwaitableCommand(Func<Task> executeMethod, Action<Exception> exceptionHandler = null)
            :base(o=>executeMethod(), exceptionHandler)
        {

        }

    }

    public class AwaitableCommand<T> : ICommand
    {
        private readonly Func<T, Task> _executeMethod;
        private bool _isExecuting;
        private readonly Action<Exception> _exceptionHandler;

        public AwaitableCommand(Func<T, Task> executeMethod, Action<Exception> exceptionHandler = null)
        {
            _executeMethod = executeMethod;
            _exceptionHandler = exceptionHandler;
        }

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                await _executeMethod(obj);
            }
            catch (Exception ex)
            {
                _exceptionHandler?.Invoke(ex);
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
