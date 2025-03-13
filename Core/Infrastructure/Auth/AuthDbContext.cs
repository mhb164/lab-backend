namespace Laboratory.Backend.Auth;

public abstract class AuthDbContext : DbContext
{
    protected readonly string _connectionString;
    protected readonly ILogger? _logger;

    protected AuthDbContext() : base() { /* We need empty constructor dotnet ef migrations */ }
    protected AuthDbContext(ILogger? logger, string? connectionString) : base()
    {
        _logger = logger;
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserLdapAccount> UserLdapAccounts { get; set; }
    public DbSet<UserEmail> UserEmails { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<ArchivedUserToken> ArchivedUserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();

        modelBuilder.Entity<UserLdapAccount>()
            .HasIndex(ula => new { ula.Username, ula.Domain })
            .IsUnique();

        modelBuilder.Entity<UserLdapAccount>()
            .HasIndex(ula => ula.UserId);

        modelBuilder.Entity<UserEmail>()
            .HasIndex(ue => ue.Email)
            .IsUnique();

        modelBuilder.Entity<UserEmail>()
            .HasIndex(ue => ue.UserId);

        modelBuilder.Entity<ArchivedUserToken>()
            .HasIndex(x => x.UserId);
    }

    public abstract Task InitialAsync();
    public abstract bool IsUniqueConstraintViolation(DbUpdateException ex);

}