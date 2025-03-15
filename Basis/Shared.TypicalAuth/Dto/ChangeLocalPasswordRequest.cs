namespace Shared.Dto;

public partial class ChangeLocalPasswordRequest
{
    public ChangeLocalPasswordRequest() { /* Method intentionally left empty.*/ }
    public ChangeLocalPasswordRequest(string? currentPassword, string? newPassword)
    {
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
