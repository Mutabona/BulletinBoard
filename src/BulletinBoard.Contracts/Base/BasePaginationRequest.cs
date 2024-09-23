namespace BulletinBoard.Contracts.Base;

/// <summary>
/// Базовый запрос с пагинацией ответа.
/// </summary>
public class BasePaginationRequest
{
    /// <summary>
    /// Кол-во элементов для получения.
    /// </summary>
    public int Take { get; set; }
    
    /// <summary>
    /// Кол-во элементов для пропуска перед получением.
    /// </summary>
    public int? Skip { get; set; }
}