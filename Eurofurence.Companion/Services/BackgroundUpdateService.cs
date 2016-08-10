using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Services.Abstractions;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(
        Scope = IocBeacon.ScopeEnum.Singleton, 
        Environment = IocBeacon.EnvironmentEnum.Any, 
        TargetType = typeof(IBackgroundUpdateService))]
    public class BackgroundUpdateService : IBackgroundUpdateService
    {
        private readonly ContextManager _contextManager;
        private readonly DispatcherTimer _timer;
        
        public BackgroundUpdateService(ContextManager contextManager)
        {
            _contextManager = contextManager;
            _timer = new DispatcherTimer();
            _timer.Tick += TimerOnTickAsync;
            
        }

        private async void TimerOnTickAsync(object sender, object o)
        {
            if (_contextManager.UpdateStatus == TaskStatus.Running) return;
            await _contextManager.Update(doSaveToStoreBeforeUpdate: true);
        }

        public void Start(TimeSpan interval)
        {
            _timer.Interval = interval;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
