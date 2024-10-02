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
    public string? Search { get; set; }
    
    /// <summary>
    /// Минимальная цена.
    /// </summary>
    public decimal? MinPrice { get; set; }
    
    /// <summary>
    /// Максимальная цена.
    /// </summary>
    public decimal? MaxPrice { get; set; }
}