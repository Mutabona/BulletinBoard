namespace BulletinBoard.Contracts.Bulletins;

/// <summary>
/// Запрос на обновление категории.
/// </summary>
public class UpdateBulletinRequest
{
    /// <summary>
    /// Название объявления.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Описание объявления.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Идентификатор категории товара.
    /// </summary>
    public Guid? CategoryId { get; set; }
    
    /// <summary>
    /// Цена товара.
    /// </summary>
    public decimal? Price { get; set; }
}