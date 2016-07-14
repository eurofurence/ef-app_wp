using System;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataStore.Abstractions;

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

        public string LastPackageVersionRunning
        {
            get { return _applicationSettingsManager.Get<string>("LastPackageVersionRunning", "-"); }
            set { _applicationSettingsManager.Set<string>("LastPackageVersionRunning", value); }
        }

        public DateTime? LastServerQueryDateTimeUtc
        {
            get { return _applicationSettingsManager.Get<DateTime?>("LastServerQueryDateTimeUtc", null); }
            set { _applicationSettingsManager.Set("LastServerQueryDateTimeUtc", value); }
        }

        public Guid UniqueRandomUserId
        {
            get
            {
                Guid? id = _applicationSettingsManager.Get<Guid?>("UniqueRandomUserId", null);
                if (!id.HasValue)
                {
                    id = Guid.NewGuid();
                    _applicationSettingsManager.Set("UniqueRandomUserId", id);
                }

                return id.Value;
            }

        }
    }
}