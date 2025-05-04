namespace Shared.Crypto;

public class CryptoRequest
{
    public CryptoRequest() { /* Method intentionally left empty.*/ }

    public CryptoRequest(CryptoOperation operation, string content, string password, int iterations)
    {
        Operation = Operation;
        Content = content;
        Password = password;
    }

    public CryptoOperation? Operation { get; set; }
    public string? Content { get; set; }
    public string? Password { get; set; }
    public int? Iterations { get; set; }

    public ServiceResult Validate()
    {
        if (Operation is null)
            return ServiceResult.BadRequest($"{nameof(Operation)} is required!");

        if (string.IsNullOrWhiteSpace(Content))
            return ServiceResult.BadRequest($"{nameof(Content)} is required!");

        if (string.IsNullOrWhiteSpace(Password))
            return ServiceResult.BadRequest($"{nameof(Password)} is required!");

        if (Iterations is null)
            return ServiceResult.BadRequest($"{nameof(Iterations)} is required!");

        return ServiceResult.Success();
    }
}
