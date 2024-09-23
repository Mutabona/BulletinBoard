using System.Linq.Expressions;

namespace BulletinBoard.AppServices.Specifications;

/// <summary>
/// Маркерный интерфейс спецификации.
/// </summary>
public interface ISpecification { }
    
/// <summary>
/// Спецификация некоторой сущности.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface ISpecification<TEntity> : ISpecification
{
    /// <summary>
    /// Дерево выражений предиката спецификации.
    /// </summary>
    Expression<Func<TEntity, bool>> PredicateExpression { get; }
    /// <summary>
    /// Возвращает признак того, что объект удовлетворяет спецификации.
    /// </summary>
    /// <param name="entity">Объект для валидации.</param>
    /// <returns>Признак того, что объект удовлетворяет спецификации.</returns>
    bool IsSatisfiedBy(TEntity entity);
}