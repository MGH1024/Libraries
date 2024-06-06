using Api;
using Application;
using Infrastructures;
using Persistence;
using Persistence.BackgroundJobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiService();
builder.AddPersistenceService();
builder.AddApplicationServices();
builder.AddInfrastructuresServices();
builder.RegisterApp();