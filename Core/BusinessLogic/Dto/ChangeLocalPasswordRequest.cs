namespace Laboratory.Backend.Dto;

public class ChangeLocalPasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
