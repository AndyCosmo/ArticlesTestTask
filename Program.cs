using ArticlesTestTask.DAL;
using ArticlesTestTask.DAL.Repository;
using ArticlesTestTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ArticleContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<IArticleService, ArticleService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Articles API",
            Version = "v1"
        }
     );

    var filePath = Path.Combine(AppContext.BaseDirectory, "ArticlesTestTask.xml");
    c.IncludeXmlComments(filePath);
});
builder.Services.AddControllersWithViews()
                      .AddJsonOptions(o => o.JsonSerializerOptions
                      .ReferenceHandler = ReferenceHandler.Preserve);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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
