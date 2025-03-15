namespace Shared.Dto;

public class SignInRequest
{
    public SignInRequest() { /* Method intentionally left empty.*/ }
    public SignInRequest(string? username, string? password)
    {
        Username = username;
        Password = password;
    }

    public string? Username { get; set; }
    public string? Password { get; set; }

    public ServiceResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
            return ServiceResult.BadRequest($"{nameof(Username)} is required!");

        if (string.IsNullOrWhiteSpace(Password))
            return ServiceResult.BadRequest($"{nameof(Password)} is required!");

        return ServiceResult.Success();
    }
}
