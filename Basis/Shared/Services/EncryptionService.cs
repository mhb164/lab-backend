using Shared.Interfaces;
using System.Text;

namespace Shared.Services;

public class EncryptionService
{
    private readonly ICryptoService _crypto;

    public EncryptionService(ICryptoService crypto)
    {
        _crypto = crypto;
    }

    public async Task<string> EncryptAsync(string plaintext, string password, int iterations = 100_000)
    {
        var encryptedBytes = await EncryptAsync(Encoding.UTF8.GetBytes(plaintext), Encoding.UTF8.GetBytes(password), iterations);
        return ToString(encryptedBytes);
    }

    public async Task<byte[]> EncryptAsync(byte[] plaintextBytes, byte[] passwordBytes, int iterations = 100_000)
    {
        var randomBytes = await _crypto.GenerateBytesAsync(28);
        var salt = new byte[16];
        var iv = new byte[12];

        Buffer.BlockCopy(randomBytes, 0, salt, 0, 16);
        Buffer.BlockCopy(randomBytes, 16, iv, 0, 12);

        var encryptedBytes = await _crypto.EncryptGCMAsync(plaintextBytes, passwordBytes, salt, iv, iterations);

        byte[] result = new byte[salt.Length + iv.Length + encryptedBytes.Length];
        Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
        Buffer.BlockCopy(iv, 0, result, salt.Length, iv.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, salt.Length + iv.Length, encryptedBytes.Length);
        return result;
    }

    public async Task<string> DecryptAsync(string encrypted, string password, int iterations = 100_000)
    {
        var encryptedBytes = ToBytes(encrypted);
        var decryptedBytes = await DecryptAsync(encryptedBytes, Encoding.UTF8.GetBytes(password), iterations);
        return Encoding.UTF8.GetString(decryptedBytes);
    }


    public async Task<byte[]> DecryptAsync(byte[] encryptedBytes, byte[] passwordBytes, int iterations = 100_000)
    {
        var salt = new byte[16];
        var iv = new byte[12];
        var bytes = new byte[encryptedBytes.Length - 28];

        Buffer.BlockCopy(encryptedBytes, 0, salt, 0, 16);  // First 16 bytes are salt
        Buffer.BlockCopy(encryptedBytes, 16, iv, 0, 12);   // Next 12 bytes are IV
        Buffer.BlockCopy(encryptedBytes, 28, bytes, 0, bytes.Length);

        return await _crypto.DecryptGCMAsync(bytes, passwordBytes, salt, iv, iterations);
    }


    private static byte[] ToBytes(string text)
    {
        return FromHexString(text);
        //return Convert.FromBase64String(text);
    }

    private static string ToString(byte[] bytes)
    {
        return ToHexString(bytes);
        //return Convert.ToBase64String(bytes);
    }

    private static string ToHexString(byte[] bytes)
    {
        if (bytes == null)
            throw new ArgumentNullException(nameof(bytes));

        char[] c = new char[bytes.Length * 2];
        byte b;
        for (int i = 0; i < bytes.Length; i++)
        {
            b = ((byte)(bytes[i] >> 4));
            c[i * 2] = (char)(b > 9 ? b + 55 : b + 48);
            b = ((byte)(bytes[i] & 0xF));
            c[i * 2 + 1] = (char)(b > 9 ? b + 55 : b + 48);
        }
        return new string(c);
    }

    private static byte[] FromHexString(string hex)
    {
        if (hex == null)
            throw new ArgumentNullException(nameof(hex));
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Hex string must have even length.", nameof(hex));

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
