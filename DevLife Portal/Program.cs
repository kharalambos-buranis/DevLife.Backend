using DevLife_Portal.Common.Extensions;
using DevLife_Portal.Common.Services;
using DevLife_Portal.Features.Dashboard;
using DevLife_Portal.Infrastructure.Database.PostgreSQL;
using DevLife_Portal.Infrastructure.Mongo;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Net.Http.Headers;
using static DevLife_Portal.Features.Auth.RegisterUser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

var mongoSettings = builder.Configuration.GetSection("MongoDb");
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = new MongoClient(mongoSettings["ConnectionString"]);
    return client.GetDatabase(mongoSettings["Database"]);
});
builder.Services.AddSingleton<MongoSeeder>();

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<ZodiacService>();

builder.Services.AddValidatorsFromAssemblyContaining<Validator>();
builder.Services.AddEndpoints();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ICodeWarsChallengeService, CodeWarsChallengeService>();
builder.Services.AddScoped<ICodeExecutionService, MockCodeExecutionService>();
builder.Services.AddScoped<IAiContentService, MockAiContentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName.Replace('+', '_'));
});

var app = builder.Build();

// Configure the HTTP request pipeline.


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var mongoSeeder = scope.ServiceProvider.GetRequiredService<MongoSeeder>();
    await mongoSeeder.SeedAsync();
}

app.UseHttpsRedirection();

app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();


app.Run();


