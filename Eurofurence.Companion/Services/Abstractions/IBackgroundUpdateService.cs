using System;

namespace Eurofurence.Companion.Services.Abstractions
{
    public interface IBackgroundUpdateService
    {
        void Start(TimeSpan interval);
        void Stop();
    }
}