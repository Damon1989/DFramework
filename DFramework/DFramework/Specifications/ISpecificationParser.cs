namespace DFramework.Specifications
{
    public interface ISpecificationParser<T>
    {
        T Parse<TEntity>(ISpecification<TEntity> specification);
    }
}