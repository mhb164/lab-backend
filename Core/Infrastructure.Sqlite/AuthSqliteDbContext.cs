namespace Laboratory.Backend;

public class AuthSqliteDbContext : AuthDbContext
{
    public AuthSqliteDbContext() : base() { /* We need empty constructor dotnet ef migrations */ }
    public AuthSqliteDbContext(ILogger<AuthSqliteDbContext>? logger, string? connectionString)
        : base(logger, connectionString) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
        var builder = new SqliteConnectionStringBuilder(_connectionString);
    }


    public override async Task InitialAsync()
    {
        await Database.MigrateAsync();
    }

    public override bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx)
        {
            // SQLite unique constraint violation error code is 2067
            return sqliteEx.SqliteErrorCode == 2067;
        }

        return false;
    }
}
