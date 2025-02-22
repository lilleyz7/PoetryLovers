using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using PoetryLovers.Data;
using PoetryLovers.Entities;
using PoetryLovers.IServices;
using PoetryLovers.Services;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;


const string DevelopmentPolicy = "_devPolicy";
const string ProductionPolicy = "_productionPolicy";

var builder = WebApplication.CreateBuilder(args);

//TODOS
// Add Logging

// SETUP RATE LIMITING
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "postLimiter", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(30);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 4;
    }));

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "getLimiter", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(90);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var env = builder.Environment;

if (env.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: DevelopmentPolicy,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:5753",
                                                  "https://localhost:5753");
                          });
    });
    builder.Services.AddDbContext<PoemContext>(opt => {
        var connectionString = builder.Configuration.GetConnectionString("Sqlite");
        opt.UseSqlite(connectionString);
    });
}
else if (env.IsProduction())
{
    var productionOrigin = builder.Configuration["ProductionOriginUrl"];
    if (productionOrigin is null)
    {
        throw new Exception("No production url available");
    }
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: ProductionPolicy,
                          policy =>
                          {
                              policy.WithOrigins(productionOrigin);
                          });
    });

    builder.Services.AddDbContext<PoemContext>(opt => {
        var connectionString = builder.Configuration.GetConnectionString("Production");
        opt.UseSqlServer(connectionString);
    });
}

builder.Services.AddHttpClient<PoetryDbService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://poetrydb.org/");
});

builder.Services.AddScoped<IPoemRepo, PoemRepo>();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<PoemContext>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRateLimiter();

app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapHealthChecks("/health");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
