using System;

namespace Eurofurence.Companion.DataStore
{
    public class ApplicationSettingsContext
    {
        private IApplicationSettingsManager _applicationSettingsManager;


        public DateTime? LastServerQueryDateTimeUtc {
            get
            {
                return _applicationSettingsManager.Get<DateTime?>("LastServerQueryDateTimeUtc", null);
            }
            set
            {
                _applicationSettingsManager.Set("LastServerQueryDateTimeUtc", value);
            }
        } 


        public ApplicationSettingsContext(IApplicationSettingsManager applicationSettingsManager)
        {
            _applicationSettingsManager = applicationSettingsManager;
        }
    }
}
