using System.Collections.Generic;
using System.Linq;
using Eurofurence.Companion.DataModel.Abstractions;

namespace Eurofurence.Companion.DataModel
{
    public class ExtensionProxy<TApi, TLocal>
            where TApi : EntityBase
            where TLocal : EntityBase, IEntityExtension, new()
    {
        private readonly TApi _apiEntity;
        private readonly ICollection<TLocal> _extensionStore;

        public ExtensionProxy(ICollection<TLocal> extensionStore, TApi apiEntity)
        {
            _extensionStore = extensionStore;
            _apiEntity = apiEntity;
        }

        public TLocal Extension
        {
            get
            {
                var result = _extensionStore.SingleOrDefault(local => local.Id == _apiEntity.Id);

                if (result == null)
                {
                    result = new TLocal() { Id = _apiEntity.Id };
                    _extensionStore.Add(result);
                }

                return result;
            }
        }
    }
}