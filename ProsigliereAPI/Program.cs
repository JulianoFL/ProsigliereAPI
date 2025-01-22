
using AutoMapper;
using Libraries.Models.Gateway;
using Libraries.Models.Parameters;
using LiteDB;
using ProsigliereAPI.Models.DBModels;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//This adds the XML comments of classes and routes to the doc
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    var XmlFile = $"Libraries.xml";
    var XmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, XmlFile);
    options.IncludeXmlComments(XmlPath);
});


//LiteDB for database
builder.Services.AddSingleton(sp => new LiteDatabase("Filename=ProsigliereAPIBlog.db;Connection=shared;"));

builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

//Automapper
var MConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<DBBlogPost, GwBlogPost>();
    cfg.CreateMap<DBBlogPostComment, GwBlogPostComment>();
    cfg.CreateMap<JBlogPost, DBBlogPost>();
    cfg.CreateMap<JBlogPostComment, DBBlogPostComment>();
});;
builder.Services.AddSingleton(MConfig.CreateMapper());



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
