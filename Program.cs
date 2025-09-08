using DealManagementSystem.Domain.Mapping;
using DealManagementSystem.Domain.Models;
using DealManagementSystem.Domain.Services;
using DealManagementSystem.Domain.Validators;
using DealManagementSystem.Persistence.Context;
using DealManagementSystem.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DealContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Services
builder.Services.AddScoped<IDealService, DealService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IFileService, FileService>();

//Validator
builder.Services.AddScoped<IValidator<Deal>, DealValidator>();
builder.Services.AddScoped<IValidator<Hotel>, HotelValidator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(_=>{}, typeof(DtoToModel), typeof(ModelToDto));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// mapping Uploads folder to Resources folder 
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//            Path.Combine(builder.Environment.WebRootPath, "Uploads")),
//     RequestPath = "/Resources"
// });
app.UseStaticFiles();

app.UseCors("Allow");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
