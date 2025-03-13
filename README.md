# Backend Laboratory


# Ef migrations - sample

cd core/Infrastructure.Sqlite
dotnet ef migrations add AddInitialCreate --context AuthSqliteDbContext -o Migrations/AuthDb
dotnet ef migrations add AddInitialCreate --context BusinessSqliteDbContext -o Migrations/BusinessDb

cd core/Infrastructure.Sql
dotnet ef migrations add AddInitialCreate --context AuthSqlDbContext -o Migrations/AuthDb
dotnet ef migrations add AddInitialCreate --context BusinessSqlDbContext -o Migrations/BusinessDb
