using System.Security.Cryptography;
using System.Text;

namespace back.Infrastructure.Services;

public interface IPasswordService
{
    void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
    bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
}

public class PasswordService : IPasswordService
{
    public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}