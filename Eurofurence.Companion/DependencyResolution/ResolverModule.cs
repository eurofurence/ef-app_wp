using Ninject.Modules;
using System.Reflection;
using Windows.ApplicationModel;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            var self = GetType().GetTypeInfo().Assembly;

            foreach(var @type in self.DefinedTypes)
            {
                var beacon = @type.GetCustomAttribute<IocBeacon>(true);
                if (beacon == null) continue;

                if (beacon.Environment == IocBeacon.EnvironmentEnum.RunTimeOnly 
                    && DesignMode.DesignModeEnabled) continue;
                if (beacon.Environment == IocBeacon.EnvironmentEnum.DesignTimeOnly 
                    && !DesignMode.DesignModeEnabled) continue;

                var binder = Bind(beacon.TargetType ?? @type.AsType()).To(@type.AsType());

                switch (beacon.Scope)
                {
                    case IocBeacon.ScopeEnum.Singleton:
                        binder.InSingletonScope();
                        break;
                    case IocBeacon.ScopeEnum.Transient:
                        binder.InTransientScope();
                        break;

                }
            }
        }
    }
}

