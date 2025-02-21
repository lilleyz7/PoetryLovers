using Microsoft.EntityFrameworkCore;
using PoetryLovers.Data;
using PoetryLovers.Entities;
using PoetryLovers.IServices;
using PoetryLovers.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var env = builder.Environment;

if (env.IsDevelopment())
{
    builder.Services.AddDbContext<PoemContext>(opt => {
        var connectionString = builder.Configuration.GetConnectionString("Sqlite");
        opt.UseSqlite(connectionString);
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
