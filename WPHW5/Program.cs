using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WPHW5.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvcCore();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "My API",
        Description = "lonh and very interesting description might be here next time",
        TermsOfService = new Uri("https://learn.microsoft.com/"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "CoolDeveloper",
            Email = "example@gmail.com",
            Url = new Uri("https://learn.microsoft.com/")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "license",
            Url = new Uri("https://learn.microsoft.com/")
        }
    });
});

builder.Services.AddDbContext<WeatherContext>(x => x.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\77071\\source\\repos\\WPHW\\WPHW5\\Data\\WeatherDB.mdf;Integrated Security=True"));

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
