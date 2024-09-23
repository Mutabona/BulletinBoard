namespace BulletinBoard.Contracts.Categories;

public class CreateCategoryRequest
{
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Идентификатор материнской категории.
    /// </summary>
    public Guid ParentCategoryId { get; set; }
}