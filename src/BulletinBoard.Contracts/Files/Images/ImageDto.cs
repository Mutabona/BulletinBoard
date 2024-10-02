namespace BulletinBoard.Contracts.Files.Images;

/// <summary>
/// Фото.
/// </summary>
public class ImageDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор объявления с изображением.
    /// </summary>
    public Guid BulletinId { get; set; }
    
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
}