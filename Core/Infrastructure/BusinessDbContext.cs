namespace Laboratory.Backend;

public abstract class BusinessDbContext : DbContext
{
    protected readonly string _connectionString;
    protected readonly ILogger? _logger;

    protected BusinessDbContext() : base() { /* We need empty constructor dotnet ef migrations */ }
    protected BusinessDbContext(ILogger? logger, string? connectionString) : base()
    {
        _logger = logger;
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductType>()
            .HasIndex(x => x.Name)
            .IsUnique();
    }

    public abstract Task InitialAsync();
    public abstract bool IsUniqueConstraintViolation(DbUpdateException ex);

}