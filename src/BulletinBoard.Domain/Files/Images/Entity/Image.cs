using BulletinBoard.Domain.Base;
using BulletinBoard.Domain.Bulletins.Entity;

namespace BulletinBoard.Domain.Files.Images.Entity;

/// <summary>
/// Сущность фото.
/// </summary>
public class Image : BaseEntity
{
    /// <summary>
    /// Контент фото.
    /// </summary>
    public byte[] Content { get; set; }

    /// <summary>
    /// Тип контента фото.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Размер фото.
    /// </summary>
    public int Length { get; set; }
    
    /// <summary>
    /// Идентификатор объявления с изображением.
    /// </summary>
    public Guid BulletinId { get; set; }
    
    /// <summary>
    /// Объявление с изображением.
    /// </summary>
    public virtual Bulletin Bulletin { get; set; }
}