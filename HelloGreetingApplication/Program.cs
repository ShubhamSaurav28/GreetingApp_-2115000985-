using BusinessLayer.Interface;
using BusinessLayer.Service;
using NLog.Extensions.Logging;
using NLog.Web;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;


var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddNLog();
});
var logger = loggerFactory.CreateLogger<Program>();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.LogError(ex, "Application stopped due to an exception.");
    throw;
}
