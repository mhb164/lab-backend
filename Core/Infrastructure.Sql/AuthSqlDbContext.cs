using Laboratory.Backend.Auth;

namespace Laboratory.Backend;

public class AuthSqlDbContext : AuthDbContext
{
    public AuthSqlDbContext() : base() { /* We need empty constructor dotnet ef migrations */ }
    public AuthSqlDbContext(ILogger<AuthSqlDbContext>? logger, string? connectionString)
        : base(logger, connectionString) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
        var builder = new SqlConnectionStringBuilder(_connectionString);
        InitialCatalog = builder.InitialCatalog;
    }

    public string? InitialCatalog { get; private set; }

    public override async Task InitialAsync()
    {
        await Database.MigrateAsync();
        if (!string.IsNullOrWhiteSpace(InitialCatalog))
            await Database.ExecuteSqlRawAsync($"ALTER DATABASE [{InitialCatalog}] SET RECOVERY SIMPLE;");
    }

    public override bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // Check if the exception is due to unique constraint violation
        // Implementation may vary depending on the database provider

        return ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601);
    }

}
