using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Core.Interfaces;
using SuperHeroAPI.Infrastructure.Data;
using SuperHeroAPI.Infrastructure.Repositories;  // Or the appropriate namespace for your repository

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register AutoMapper and specify the assembly from the Application Layer
builder.Services.AddAutoMapper(typeof(SuperHeroAPI.Application.Mappings.SuperHeroProfile).Assembly);

// Register MediatR
builder.Services.AddMediatR(typeof(SuperHeroAPI.Application.Features.SuperHeros.Queries.GetAllSuperHeroes.GetAllSuperHeroesQueryHandler).Assembly);

// Register the repository
builder.Services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext for SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
