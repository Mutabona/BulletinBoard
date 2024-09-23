using BulletinBoard.Contracts.Base;

namespace BulletinBoard.Contracts.Bulletins;

/// <summary>
/// Модель поиска объявлений.
/// </summary>
public class SearchBulletinRequest : BasePaginationRequest
{
    /// <summary>
    /// Строка поиска.
    /// </summary>
    public string Search { get; set; }
}