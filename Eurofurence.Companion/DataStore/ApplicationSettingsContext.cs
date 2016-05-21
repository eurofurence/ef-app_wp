using Eurofurence.Companion.DependencyResolution;
using System;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(ApplicationSettingsContext))]
    public class ApplicationSettingsContext
    {
        private readonly IApplicationSettingsManager _applicationSettingsManager;


        public ApplicationSettingsContext(IApplicationSettingsManager applicationSettingsManager)
        {
            _applicationSettingsManager = applicationSettingsManager;
        }


        public DateTime? LastServerQueryDateTimeUtc
        {
            get { return _applicationSettingsManager.Get<DateTime?>("LastServerQueryDateTimeUtc", null); }
            set { _applicationSettingsManager.Set("LastServerQueryDateTimeUtc", value); }
        }
    }
}