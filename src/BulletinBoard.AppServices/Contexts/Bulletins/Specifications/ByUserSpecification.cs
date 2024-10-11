using System.Linq.Expressions;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Specifications;

/// <summary>
/// Спецификация отсечения по пользователю.
/// </summary>
public class ByUserSpecification : Specification<Bulletin>
{
    private readonly Guid _userId;
        
    /// <summary>
    /// Создаёт спецификацию отсечения по пользователю <see cref="ByUserSpecification"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public ByUserSpecification(Guid userId)
    {
        _userId = userId;
    }

    public override Expression<Func<Bulletin, bool>> PredicateExpression => 
        bulletin => bulletin.OwnerId <= _userId;
}