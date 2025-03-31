# Backend Laboratory

## Ef migrations Business - sample
cd core/Infrastructure.Concrete
dotnet ef migrations add AddInitialCreateSql --context BusinessSqlDbContext -o Migrations/Sql
dotnet ef migrations add AddInitialCreateSqlite --context BusinessSqliteDbContext -o Migrations/Sqlite

## Ef migrations TypicalAuth - sample
cd TypicalAuth/TypicalAuth.Infrastructure.Concrete
dotnet ef migrations add AddInitialCreateSql --context AuthSqlDbContext -o Migrations/Sql
dotnet ef migrations add AddInitialCreateSqlite --context AuthSqliteDbContext -o Migrations/Sqlite

# Windows Service
## Create the Windows Service
sc create LabBackend binPath="path to Laboratory.Backend.exe" start=auto  DisplayName="Backend Laboratory"

## Start the Service
sc start LabBackend

## Verify the Service is Running
sc query LabBackend

## Stop & Delete the Service
sc stop LabBackend
sc delete LabBackend

curl https://localhost/labapi/info