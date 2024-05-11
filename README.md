# Libraries



dotnet ef migrations add --project Microservices/LibraryMicroservice/LibraryMicroservice.Persistence/LibraryMicroservice.Persistence.csproj --startup-project Microservices/LibraryMicroservice/LibraryMicroservice.Api/LibraryMicroservice.Api.csproj --context Persistence.Contexts.LibraryDbContext --configuration Debug Initial --output-dir Migrations

dotnet ef database update --project Microservices/LibraryMicroservice/LibraryMicroservice.Persistence/LibraryMicroservice.Persistence.csproj --startup-project Microservices/LibraryMicroservice/LibraryMicroservice.Api/LibraryMicroservice.Api.csproj --context Persistence.Contexts.LibraryDbContext --configuration Debug 20240511151100_Initial



