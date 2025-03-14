# Backend Laboratory

## Ef migrations Business - sample
cd core/Infrastructure.Concrete
dotnet ef migrations add AddInitialCreateSql --context BusinessSqlDbContext -o Migrations/Sql
dotnet ef migrations add AddInitialCreateSqlite --context BusinessSqliteDbContext -o Migrations/Sqlite

## Ef migrations TypicalAuth - sample
cd TypicalAuth/TypicalAuth.Infrastructure.Concrete
dotnet ef migrations add AddInitialCreateSql --context AuthSqlDbContext -o Migrations/Sql
dotnet ef migrations add AddInitialCreateSqlite --context AuthSqliteDbContext -o Migrations/Sqlite