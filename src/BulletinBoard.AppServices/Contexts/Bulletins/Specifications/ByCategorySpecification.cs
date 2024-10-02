using System.Linq.Expressions;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Specifications;

public class ByCategorySpecification : Specification<Bulletin>
{
    private readonly ICollection<Guid?> _categoriesIds;

    /// <summary>
    /// Инициализирует экземпляр <see cref="ByCategorySpecification"/>.
    /// </summary>
    public ByCategorySpecification(ICollection<Guid?> categoriesIds)
    {
        _categoriesIds = categoriesIds;
    }

    /// <inheritdoc />
    public override Expression<Func<Bulletin, bool>> PredicateExpression => bulletin => 
        _categoriesIds.Contains(bulletin.CategoryId);
}