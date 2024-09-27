using BulletinBoard.Contracts.Categories;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Contracts.Users;

namespace BulletinBoard.Contracts.Bulletins;

/// <summary>
/// Объявление.
/// </summary>
public class BulletinDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid? Id { get; set; }
   
    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid? OwnerId { get; set; }
    
    /// <summary>
    /// Имя владельца.
    /// </summary>
    public string OwnerName { get; set; }
    
    /// <summary>
    /// Фамилия владельца.
    /// </summary>
    public string OwnerSurname { get; set; }
    
    /// <summary>
    /// Отчество владельца. 
    /// </summary>
    public string? OwnerLastname { get; set; }
    
    /// <summary>
    /// Название объявления.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Описание объявления.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Идентификатор категории товара.
    /// </summary>
    public Guid? CategoryId { get; set; }
    
    /// <summary>
    /// Категория товара.
    /// </summary>
    public string CategoryName { get; set; }
    
    /// <summary>
    /// Цена товара.
    /// </summary>
    public decimal? Price { get; set; }
}