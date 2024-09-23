using System.Linq.Expressions;

namespace BulletinBoard.AppServices.Specifications.Internal;

/// <summary>
/// Обобшенная спецификация на основе дерева выражений.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
internal class ExpressionSpecification<TEntity> : Specification<TEntity>
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="ExpressionSpecification{TEntity}"/>.
    /// </summary>
    public ExpressionSpecification(Expression<Func<TEntity, bool>> expression)
    {
        PredicateExpression = expression ?? throw new ArgumentNullException(nameof(expression));
    }
    /// <inheritdoc />
    public override Expression<Func<TEntity, bool>> PredicateExpression { get; }
}