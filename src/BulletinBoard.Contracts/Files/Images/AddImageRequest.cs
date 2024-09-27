using Microsoft.AspNetCore.Http;

namespace BulletinBoard.Contracts.Files.Images;

/// <summary>
/// Запрос на добавление картинки.
/// </summary>
public class AddImageRequest
{
    /// <summary>
    /// Картинка.
    /// </summary>
    public IFormFile Image { get; set; }
    
    /// <summary>
    /// Идентификатор объявления с изображением.
    /// </summary>
    public Guid? BulletinId { get; set; }
}