namespace MA.Patterns.Factory
{
    public interface IFactoryGeneric<ProductT>
    {
        public ProductT Create();
    }
}
