namespace BulletinBoard.Contracts.Categories;

/// <summary>
/// Категория.
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
   
    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Идентификатор материнской категории.
    /// </summary>
    public Guid ParentCategoryId { get; set; }
}