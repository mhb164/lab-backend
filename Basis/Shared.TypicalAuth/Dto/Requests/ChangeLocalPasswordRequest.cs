namespace Shared.Dto.Requests;

public partial class ChangeLocalPasswordRequest
{
    public ChangeLocalPasswordRequest() { /* Method intentionally left empty.*/ }
    public ChangeLocalPasswordRequest(string? currentPassword, string? newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword))
            throw new ArgumentException($"{nameof(currentPassword)} is required!", nameof(currentPassword));

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException($"{nameof(newPassword)} is required!", nameof(newPassword));

        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }

    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }

    public ServiceResult Validate()
    {
        if (string.IsNullOrWhiteSpace(CurrentPassword))
            return ServiceResult.BadRequest($"{nameof(CurrentPassword)} is required!");

        if (string.IsNullOrWhiteSpace(NewPassword))
            return ServiceResult.BadRequest($"{nameof(NewPassword)} is required!");

        return ServiceResult.Success();
    }
}
