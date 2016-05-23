using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.DataModel
{
    public class ExtensionProxy<TApi, TLocal>
            where TApi : EntityBase
            where TLocal : IEntityExtension, new()
    {
        public ExtensionProxy()
        {

                
        }
    }
}