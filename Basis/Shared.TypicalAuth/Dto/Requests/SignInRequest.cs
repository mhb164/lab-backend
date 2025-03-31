namespace Shared.Dto.Requests;

public class SignInRequest
{
    public SignInRequest() { /* Method intentionally left empty.*/ }
    public SignInRequest(string? type, string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException($"{nameof(type)} is required!", nameof(type));

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException($"{nameof(username)} is required!", nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException($"{nameof(password)} is required!", nameof(password));

        Type = type;
        Username = username;
        Password = password;
    }

    public string? Type { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public ServiceResult Validate(Func<string, bool> isTypeValid)
    {
        if (string.IsNullOrWhiteSpace(Type))
            return ServiceResult.BadRequest($"{nameof(Type)} is required!");

        if (!isTypeValid(Type!))
            return ServiceResult.BadRequest($"{nameof(Type)}[{Type}] is not valid!");

        if (string.IsNullOrWhiteSpace(Username))
            return ServiceResult.BadRequest($"{nameof(Username)} is required!");

        if (string.IsNullOrWhiteSpace(Password))
            return ServiceResult.BadRequest($"{nameof(Password)} is required!");

        return ServiceResult.Success();
    }
}
