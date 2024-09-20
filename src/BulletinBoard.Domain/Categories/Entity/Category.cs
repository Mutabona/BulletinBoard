using BulletinBoard.Domain.Base;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.Domain.Categories.Entity;

/// <summary>
/// Сущность категории.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Идентификатор материнской категории.
    /// </summary>
    public Guid ParentCategoryId { get; set; }
    
    /// <summary>
    /// Материнская категория.
    /// </summary>
    public virtual Category ParentCategory { get; set; }
    
    /// <summary>
    /// Дочерние категории.
    /// </summary>
    public virtual ICollection<Category> SubCategories { get; set; }
    
    /// <summary>
    /// Объявления в категории.
    /// </summary>
    public virtual ICollection<Bulletin> Bulletins { get; set; }
}