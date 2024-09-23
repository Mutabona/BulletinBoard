using System.Security.Cryptography;
using System.Text;

namespace BulletinBoard.AppServices.Helpers;

public static class CryptoHelper
{
    public static string GetBase64Hash(string stringToEncrypt)
    {
        var buffer = Encoding.UTF8.GetBytes(stringToEncrypt);
        HashAlgorithm sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(buffer);

        return Convert.ToBase64String(hash);
    }
}