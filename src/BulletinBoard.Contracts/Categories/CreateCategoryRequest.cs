namespace BulletinBoard.Contracts.Categories;

/// <summary>
/// Модель запроса на создание категории.
/// </summary>
public class CreateCategoryRequest
{
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Идентификатор материнской категории.
    /// </summary>
    public Guid? ParentCategoryId { get; set; }
}