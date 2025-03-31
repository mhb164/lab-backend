using Shared.Dto.Requests;

namespace TypicalAuth.Interfaces;

public interface IAuthService
{
    SignInOptions GetSignInOptions();
    Task<ServiceResult<Token>> SignInAsync(SignInRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<Token>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken);
    Task<ServiceResult> SignOutAsync(CancellationToken cancellationToken);
    Task EnsureDefaultUsersExistsAsync();
    Task<ServiceResult> ChangeLocalPasswordRequestAsync(ChangeLocalPasswordRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<UserInfo>> GetUserInfoAsync(CancellationToken cancellationToken);
}
