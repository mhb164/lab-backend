namespace TypicalAuth.Services;

public class AuthService : IAuthService
{
    private readonly ILogger? _logger;
    private readonly AuthConfig _config;
    private readonly IAuthUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserTokenRepository _tokenRepository;
    private readonly IUserTokenObsoleteRepository _tokenHistoryRepository;
    private readonly JwtConfig _jwt;
    private readonly ClientUser? _clientUser;

    public AuthService(ILogger<AuthService>? logger, AuthConfig config,
        IAuthUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUserTokenRepository tokenRepository,
        IUserTokenObsoleteRepository tokenHistoryRepository,
        JwtConfig jwtConfig,
        IUserContext userContext)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        _tokenHistoryRepository = tokenHistoryRepository ?? throw new ArgumentNullException(nameof(tokenHistoryRepository));
        _jwt = jwtConfig ?? throw new ArgumentNullException(nameof(jwtConfig));
        _clientUser = userContext.User;
    }

    public async Task EnsureDefaultUsersExistsAsync()
    {
        foreach (var defaultUser in _config.DefaultUsers)
        {
            await EnsureExistsAsync(defaultUser);
        }
    }

    private async Task EnsureExistsAsync(AuthConfig.DefaultUser defaultUser)
    {
        var user = await _userRepository.GetByNameAsync(defaultUser.Username, CancellationToken.None);
        if (user is not null)
        {
            _logger?.LogInformation("{Username}({ActivateStatus}) user already exists.", defaultUser.Username,
                (user.Activation ? "Active" : "Inctive"));
            return;
        }

        if (string.IsNullOrWhiteSpace(defaultUser.DefaultPassword))
            throw new InvalidOperationException($"'{defaultUser.Username}' user default password required!");

        var password = defaultUser.PasswordAlreadyHashed
            ? defaultUser.DefaultPassword
            : _passwordHasher.Hash(defaultUser.DefaultPassword);

        user = new User()
        {
            Activation = true,
            Username = defaultUser.Username,
            Password = password,
            ChangePasswordRequired = defaultUser.ForceToChangePassword,
            Roles = defaultUser.Roles.ToList(),
            Firstname = defaultUser.Firstname,
            Lastname = defaultUser.Lastname,
            Nickname = defaultUser.Nickname,
            LdapAccounts = new List<UserLdapAccount>(),
            Emails = new List<UserEmail>(),
        };

        _ = await _userRepository.AddAsync(user, CancellationToken.None);
        await _unitOfWork.CommitAsync(CancellationToken.None);

        _logger?.LogInformation("{Username} user added.", defaultUser.Username);
    }

    private async Task<UserToken> AddTokenAsync(UserToken token)
    {
        var fromRepo = await _tokenRepository.AddAsync(token, CancellationToken.None);
        await _unitOfWork.CommitAsync(CancellationToken.None);

        return fromRepo!;
    }

    private async Task<ServiceResult> RemoveTokenAsync(Guid tokenId, string reason, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetByIdAsync(tokenId, cancellationToken);

        if (token is null)
            return ServiceResult.BadRequest("Token not found!");

        var userTokenObsolete = UserTokenObsolete.From(token, DateTime.UtcNow, reason);
        await _tokenRepository.DeleteAsync(token, cancellationToken);
        await _tokenHistoryRepository.AddAsync(userTokenObsolete, cancellationToken);

        await _unitOfWork.CommitAsync(CancellationToken.None);

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<Token>> SignInAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<Token>();
        var validationResult = request.Validate();
        if(validationResult.IsFailed)
            serviceResult.BadRequest(validationResult.Message!);

        var validation = await ValidateCredentials(request, cancellationToken);
        var authType = ClientAuthType.Locally;
        if (validation.Value is null || validation.IsFailed)
            return serviceResult.Unauthorized($"Validate credentials failed ({validation.Message})");
        var user = validation.Value!;

        var tokenId = Guid.NewGuid();
        var newToken = new UserToken()
        {
            Id = tokenId,
            Type = authType,
            Username = request.Username!,
            Time = DateTime.UtcNow,
            UserId = user.Id,
            Description = "",//Extra info
            RefreshToken = GenerateRefreshToken(tokenId),
            RefereshedAt = DateTime.UtcNow,
        };

        var userToken = await AddTokenAsync(newToken);
        var token = new Token(GenerateAccessToken(userToken, user), userToken.RefreshToken);
        return serviceResult.Success(token);
    }

    public async Task<ServiceResult> ChangeLocalPasswordRequestAsync(ChangeLocalPasswordRequest request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate();
        if (validationResult.IsFailed)
            return validationResult;

        if (_clientUser is null)
            return ServiceResult.Unauthorized("User not valid!");

        var user = await _userRepository.GetByIdAsync(_clientUser.Id, cancellationToken);
        if (user is null)
            return ServiceResult.Unauthorized("User not found!");

        if (!user.Activation)
            return ServiceResult.Unauthorized("User not is not active!");

        if (!_passwordHasher.Verify(user.Password, request.CurrentPassword))
            return ServiceResult.Unauthorized("Password is not valid");

        user.Password = _passwordHasher.Hash(request.NewPassword);
        user.ChangePasswordRequired = false;
        _ = await _userRepository.UpdateAsync(user, CancellationToken.None);
        await _unitOfWork.CommitAsync(CancellationToken.None);

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<Token>> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken)
    {
        var serviceResult = new ServiceResult<Token>();
        if (string.IsNullOrWhiteSpace(refreshToken))
            return serviceResult.Unauthorized("Wrong request!");

        if (!Guid.TryParse(refreshToken, out var tokenId))
            return serviceResult.Unauthorized("Wrong request!");

        var token = await _tokenRepository.GetByIdAsync(tokenId, cancellationToken);
        if (token is null)
            return serviceResult.Unauthorized("Token not found!");

        var user = await _userRepository.GetByIdAsync(token.UserId, cancellationToken);

        if (user is null)
            return serviceResult.Unauthorized("User not found!");

        if (!user.Activation)
            return serviceResult.Unauthorized("User not is not active!");

        var accessToken = GenerateAccessToken(token, user);

        token.RefereshedAt = DateTime.UtcNow;
        await _tokenRepository.UpdateAsync(token, CancellationToken.None);
        await _unitOfWork.CommitAsync(CancellationToken.None);

        return serviceResult.Success(new Token(accessToken, token.RefreshToken));
    }

    public async Task<ServiceResult> SignOutAsync(CancellationToken cancellationToken)
    {
        if (_clientUser is null || _clientUser.Token?.GUID is null)
            return ServiceResult.Unauthorized("Already signed out!");

        return await RemoveTokenAsync(_clientUser.Token.GUID.Value, "sign_out", cancellationToken);
    }

    public string GenerateAccessToken(UserToken token, User user)
    {
        var jwtValues = _config.ToJWT(token.Id, user.Id);
        var permissions = GetPermissions(user.Roles);
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new Claim(ClaimNames.TokenId, jwtValues.TokenId),
            new Claim(ClaimNames.AuthType, token.Type.ToString().ToLower()),
            new Claim(ClaimNames.AuthDetail, token.Username),
            new Claim(ClaimNames.AuthTime, new DateTimeOffset(token.Time).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimNames.UserId, jwtValues.UserId),
            new Claim(ClaimNames.Firstname, user.Firstname),
            new Claim(ClaimNames.Lastname, user.Lastname),
            new Claim(ClaimNames.Nickname, user.Nickname),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        if (user.LocallyAvailable)
            claims.Add(new Claim(ClaimNames.LocallyAvailable, "true"));

        if (user.ChangePasswordRequired)
            claims.Add(new Claim(ClaimNames.ChangeLocalPasswordRequired, "true"));
        else
            claims.AddRange(ToClaims(permissions));

        if (_config.IsReadOnly(user.Username))
            claims.Add(new Claim(ClaimNames.ReadOnly, "true"));

        var creds = new SigningCredentials(_jwt.SecretKey, SecurityAlgorithms.HmacSha256);
        var expires = now.AddMinutes(_jwt.LifetimeInMinutes);

        var jwtToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Issuer,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public static string GenerateRefreshToken(Guid tokenId)
    {
        return tokenId.ToString("N");
    }

    private async Task<ServiceResult<User>> ValidateCredentials(SignInRequest request, CancellationToken cancellationToken)
    {
        //if (request.Username.Contains("@"))
        //    return await ValidateCredentialsOnDomain(request, cancellationToken);

        var serviceResult = new ServiceResult<User>();
        var user = await _userRepository.GetByNameAsync(request.Username, cancellationToken);

        if (user is null)
            return serviceResult.Unauthorized("Username is not valid");

        if (!user.Activation)
            return serviceResult.Unauthorized("User not is not active!");

        if (!_passwordHasher.Verify(user.Password, request.Password))
            return serviceResult.Unauthorized("Password is not valid");

        await Task.CompletedTask;
        return serviceResult.Success(user);
    }

    private async Task<ServiceResult<User>> ValidateCredentialsOnDomain(SignInRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
   
    private List<UserPermit> GetPermissions(List<UserRole> roles)
    {
        var permissions = new Dictionary<string, Dictionary<string, HashSet<string>>>();//domain->scope->permits
        foreach (var role in roles)
        {
            var rolePermits = _config.GetPermits(role);
            if (!rolePermits.Any())
                continue;

            FillRolePermits(ref permissions, rolePermits);
        }

        var result = new List<UserPermit>();
        foreach (var domainPermissions in permissions)
            foreach (var scopePermissions in domainPermissions.Value)
            {
                result.Add(new UserPermit(domainPermissions.Key, scopePermissions.Key, scopePermissions.Value));
            }

        return result;
    }

    private static void FillRolePermits(ref Dictionary<string, Dictionary<string, HashSet<string>>> permissions, IEnumerable<UserPermit> rolePermits)
    {
        foreach (var rolePermit in rolePermits)
        {
            if (!permissions.TryGetValue(rolePermit.Domain, out var domainPermissions))
            {
                domainPermissions = new Dictionary<string, HashSet<string>>();
                permissions.Add(rolePermit.Domain, domainPermissions);
            }

            if (!domainPermissions.TryGetValue(rolePermit.Scope, out var scopePermissions))
            {
                scopePermissions = new HashSet<string>();
                domainPermissions.Add(rolePermit.Scope, scopePermissions);
            }
            foreach (var permission in rolePermit.Permissions)
            {
                scopePermissions.Add(permission);
            }
        }
    }

    public static IEnumerable<Claim> ToClaims(List<UserPermit> permits)
    {
        foreach (var permit in permits)
            yield return new Claim(permit.ClaimType, permit.PermissionsAsText);
    }


}
