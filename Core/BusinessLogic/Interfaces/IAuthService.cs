namespace Laboratory.Backend.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<TokenDto>> SignInAsync(SignInRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<TokenDto>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken);
    Task<ServiceResult> SignOutAsync(CancellationToken cancellationToken);
    Task EnsureDefaultUsersExistsAsync(string? adminDefaultPassword);
    Task<ServiceResult> ChangeLocalPasswordRequestAsync(ChangeLocalPasswordRequest request, CancellationToken cancellationToken);
}
