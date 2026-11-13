using System.Collections.Generic;

namespace MA.Patterns.Factory
{
    public abstract class FactoryManagerGeneric<ProductKeyT, ProductT> : IFactoryManagerGeneric<ProductKeyT, ProductT>
    {
        protected abstract Dictionary<ProductKeyT, IFactoryGeneric<ProductT>> KeyToFactoryDict { get; }

        public ProductT Create(ProductKeyT key)
        {
            if (KeyToFactoryDict == null)
                return default;

            if (!KeyToFactoryDict.TryGetValue(key, out IFactoryGeneric<ProductT> factory))
                return default;
            
            return factory.Create();
        }
    }
}
