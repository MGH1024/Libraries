# Libraries



dotnet ef migrations add --project Microservices\Library\src\Infrustructure\Library.Persistence\Library.Persistence.csproj --startup-project Microservices\Library\src\Endpoint\Library.Api\Library.Api.csproj --context Persistence.Contexts.LibraryDbContext --configuration Debug Initial --output-dir Migrations

dotnet ef database update --project Microservices\Library\src\Infrustructure\Library.Persistence\Library.Persistence.csproj --startup-project Microservices\Library\src\Endpoint\Library.Api\Library.Api.csproj --context Persistence.Contexts.LibraryDbContext --configuration Debug 20240526080643_Initial



put /person/


post /person/_doc
{
  "name" : "meysam",
  "age" : 20
}

post /person/_doc
{
  "name" : "partow",
  "age" : 20
}



GET person/_search

post person/_update/HQuFwI8Bzh2yCnvju0pf
{
  "doc":{
    "age":3,
    "color":"white"
  }
  
}



