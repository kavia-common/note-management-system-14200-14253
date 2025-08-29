# Backend build notes

If you see errors like:
- The type or namespace name 'EntityFrameworkCore' does not exist
- The type or namespace name 'JwtBearer' does not exist
- The type or namespace name 'OpenApi' does not exist

Run:
- dotnet restore note-management-system-14200-14253/notes_app_backend/dotnet.csproj
- dotnet build note-management-system-14200-14253/notes_app_backend/dotnet.csproj
- dotnet run --project note-management-system-14200-14253/notes_app_backend/dotnet.csproj

Swagger UI: http://localhost:3001/docs
