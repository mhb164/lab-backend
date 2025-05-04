namespace Shared.Interfaces;

/// <summary>
/// Salt: 16-byte (128 bits) salt for key derivation
/// IV (Nonce): 96-bit nonce (12 bytes) - commonly used size for AES-GCM
/// </summary>
public interface ICryptoService
{
    Task<byte[]> GenerateBytesAsync(int length);
    Task<byte[]> EncryptGCMAsync(byte[] plaintextBytes, byte[] passwordBytes, byte[] salt, byte[] iv, int iterations);
    Task<byte[]> DecryptGCMAsync(byte[] encryptedBytes, byte[] passwordBytes, byte[] salt, byte[] iv, int iterations);
}