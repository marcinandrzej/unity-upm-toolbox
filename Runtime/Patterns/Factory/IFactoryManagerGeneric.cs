namespace MA.Patterns.Factory
{
    public interface IFactoryManagerGeneric<ProductKeyT, ProductT>
    {
        public ProductT Create(ProductKeyT key);
    }
}
