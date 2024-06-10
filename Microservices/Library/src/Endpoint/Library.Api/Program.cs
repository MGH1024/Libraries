using Api;
using Application;
using Infrastructures;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiService();
builder.AddPersistenceService();
builder.AddApplicationServices();
builder.AddInfrastructuresServices();
builder.RegisterApp();