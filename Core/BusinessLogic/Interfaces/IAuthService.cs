using Backend.Auth;
using Backend.Dto;
using Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<TokenDto>> SignInAsync(SignInRequest request, CancellationToken cancellationToken);
    // GenerateAccessToken(User user);
    //string GenerateRefreshToken();
    //User? ValidateCredentials(SignInDto request);
    //ClientUser? GetUser(string? accessToken);
    Task<ServiceResult<TokenDto>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken);
    Task<ServiceResult> SignOutAsync(CancellationToken cancellationToken);
    Task EnsureAdminUserExistsAsync(string? adminDefaultPassword);
    Task<ServiceResult> ChangeLocalPasswordRequestAsync(ChangeLocalPasswordRequest request, CancellationToken cancellationToken);
}
