using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend;

public class BackendSqlDbContext : BackendDbContext
{
    public BackendSqlDbContext() : base() { /* We need empty constructor dotnet ef migrations */ }
    public BackendSqlDbContext(ILogger<BackendSqlDbContext>? logger, string? connectionString)
        : base(logger, connectionString) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
        var builder = new SqlConnectionStringBuilder(_connectionString);
        InitialCatalog = builder.InitialCatalog;
    }

    public string InitialCatalog { get; private set; }

    public override async Task InitialAsync()
    {
        await Database.MigrateAsync();
        await Database.ExecuteSqlRawAsync($"ALTER DATABASE [{InitialCatalog}] SET RECOVERY SIMPLE;");

    }

    public override bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // Check if the exception is due to unique constraint violation
        // Implementation may vary depending on the database provider

        return ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601);
    }

}
