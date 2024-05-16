using Api;
using Persistence;
using Application;
using Infrastructures;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiService();
builder.AddPersistenceService();
builder.AddApplicationServices();
builder.AddInfrastructuresServices();
builder.RegisterApp();

