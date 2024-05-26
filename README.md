# Libraries



dotnet ef migrations add --project Microservices\Library\src\Infrustructure\Library.Persistence\Library.Persistence.csproj --startup-project Microservices\Library\src\Endpoint\Library.Api\Library.Api.csproj --context Persistence.Contexts.LibraryDbContext --configuration Debug Initial --output-dir Migrations

dotnet ef database update --project Microservices\Library\src\Infrustructure\Library.Persistence\Library.Persistence.csproj --startup-project Microservices\Library\src\Endpoint\Library.Api\Library.Api.csproj --context Persistence.Contexts.LibraryDbContext --configuration Debug 20240526080643_Initial



