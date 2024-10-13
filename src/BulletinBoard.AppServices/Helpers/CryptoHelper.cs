using System.Security.Cryptography;
using System.Text;

namespace BulletinBoard.AppServices.Helpers;

/// <summary>
/// Класс для шифрования данных.
/// </summary>
public static class CryptoHelper
{
    /// <summary>
    /// Шифрует строку в базовый 64 хэш.
    /// </summary>
    /// <param name="stringToEncrypt">Строка.</param>
    /// <returns>Зашифрованная строка.</returns>
    public static string GetBase64Hash(string stringToEncrypt)
    {
        var buffer = Encoding.UTF8.GetBytes(stringToEncrypt);
        HashAlgorithm sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(buffer);

        return Convert.ToBase64String(hash);
    }
}