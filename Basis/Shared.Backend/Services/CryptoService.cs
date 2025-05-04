using System.Security.Cryptography;

namespace Shared.Services;

internal class CryptoService : ICryptoService
{
    private static byte[] DeriveKey(byte[] passwordBytes, byte[] salt, int iterations)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(32); // 256-bit key for AES-256
    }

    public async Task<byte[]> GenerateBytesAsync(int length)
    {
        await Task.CompletedTask;

        var randomBytes = new byte[length];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomBytes);
        return randomBytes;
    }

    public Task<byte[]> EncryptGCMAsync(byte[] plaintextBytes, byte[] passwordBytes, byte[] salt, byte[] iv, int iterations)
    {
        var key = DeriveKey(passwordBytes, salt, iterations);

        var ciphertext = new byte[plaintextBytes.Length];
        var tag = new byte[16];

        using var aesAlg = new AesGcm(key, 16);
        aesAlg.Encrypt(iv, plaintextBytes, ciphertext, tag);

        var result = new byte[ciphertext.Length + tag.Length];
        Buffer.BlockCopy(ciphertext, 0, result, 0, ciphertext.Length);
        Buffer.BlockCopy(tag, 0, result, ciphertext.Length, tag.Length);

        return Task.FromResult(result);
    }

    public Task<byte[]> DecryptGCMAsync(byte[] encryptedBytes, byte[] passwordBytes, byte[] salt, byte[] iv, int iterations)
    {
        var key = DeriveKey(passwordBytes, salt, iterations);

        var ciphertext = new byte[encryptedBytes.Length - 16];
        var tag = new byte[16];
        Buffer.BlockCopy(encryptedBytes, 0, ciphertext, 0, ciphertext.Length);
        Buffer.BlockCopy(encryptedBytes, ciphertext.Length, tag, 0, tag.Length);

        var decrypted = new byte[ciphertext.Length];

        using var aesAlg = new AesGcm(key, 16);
        aesAlg.Decrypt(iv, ciphertext, tag, decrypted);

        return Task.FromResult(decrypted);
    }
}
