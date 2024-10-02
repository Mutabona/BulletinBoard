using System.Linq.Expressions;
using BulletinBoard.AppServices.Specifications;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.AppServices.Contexts.Bulletins.Specifications;

/// <summary>
/// Спецификация поиска объявлений по поисковой строке.
/// </summary>
public class SearchStringSpecification : Specification<Bulletin>
{
    private readonly string _searchString;

    /// <summary>
    /// Создаёт спецификацию поиска объявлений по поисковой строке <see cref="SearchStringSpecification"/>.
    /// </summary>
    /// <param name="searchString">Поисковая строка.</param>
    public SearchStringSpecification(string searchString)
    {
        _searchString = searchString;
    }

    /// <inheritdoc />
    public override Expression<Func<Bulletin, bool>> PredicateExpression =>
        bulletin =>
            bulletin.Title.ToLower().Contains(_searchString.ToLower()) ||
            bulletin.Description.ToLower().Contains(_searchString.ToLower());
}