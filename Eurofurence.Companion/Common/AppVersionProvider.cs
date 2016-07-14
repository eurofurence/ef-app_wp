using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Windows.ApplicationModel;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(IAppVersionProvider))]
    public class AppVersionProvider : IAppVersionProvider
    {
        public string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
