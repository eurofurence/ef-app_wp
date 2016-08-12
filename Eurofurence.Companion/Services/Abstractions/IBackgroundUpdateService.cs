using System;

namespace Eurofurence.Companion.Services.Abstractions
{
    public interface IBackgroundUpdateService
    {
        void Start();
        void Stop();

        void SetInterval(TimeSpan interval);
    }
}