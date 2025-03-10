using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Backend.Model;

namespace Backend;

public abstract class BackendDbContext : DbContext
{
    protected readonly string _connectionString;
    protected readonly ILogger? _logger;

    protected BackendDbContext() : base() { /* We need empty constructor dotnet ef migrations */ }
    protected BackendDbContext(ILogger? logger, string? connectionString) : base()
    {
        _logger = logger;
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLdapAccount> UserLdapAccounts { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<ArchivedUserToken> ArchivedUserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductType>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();

        modelBuilder.Entity<UserLdapAccount>()
            .HasIndex(ula => new { ula.UserId, ula.Username })
            .IsUnique();

        modelBuilder.Entity<UserLdapAccount>()
            .HasIndex(ula => ula.UserId);

        modelBuilder.Entity<ArchivedUserToken>()
            .HasIndex(x => x.UserId);

    }

    public abstract Task InitialAsync();
    public abstract bool IsUniqueConstraintViolation(DbUpdateException ex);

}