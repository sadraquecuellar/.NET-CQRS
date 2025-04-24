namespace Ambev.DeveloperEvaluation.Domain.Common.Specifications;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}
