using Microsoft.AspNetCore.Http;

namespace BulletinBoard.Contracts.Bulletins;

/// <summary>
/// Запрос на создание объявления.
/// </summary>
public class CreateBulletinRequest
{
    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>
    /// Название объявления.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Описание объявления.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Идентификатор категории товара.
    /// </summary>
    public Guid CategoryId { get; set; }
}