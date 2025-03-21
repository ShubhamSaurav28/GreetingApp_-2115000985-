using System.Text;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware;
using Middleware.GlobalExceptionHandler;
using NLog.Extensions.Logging;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using StackExchange.Redis;


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
    var jwtSettings = builder.Configuration.GetSection("Jwt");


    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>(); // Register Global Exception Filter
    });

    builder.Services.AddLogging();


    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();
    builder.Services.AddScoped<IUserRegistrationBL, UserRegistrationBL>();
    builder.Services.AddScoped<IUserRegistrationRL, UserRegistrationRL>();
    builder.Services.AddScoped<JwtMiddleware>();
    builder.Services.AddSingleton<IEmailServiceBL, EmailServiceBL>();
    builder.Services.AddSingleton<RabbitMQProducer>();
    builder.Services.AddSingleton<RabbitMQConsumer>();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<GreetingAppContext>(options => options.UseSqlServer(connectionString));

    var redisConfig = builder.Configuration.GetConnectionString("RedisConnection");
    Console.WriteLine(redisConfig);
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))

        };

    });

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "AddressBook API",
            Description = "AddressBook API Management",
            TermsOfService = new Uri("https://www.example.com/"),
            Contact = new OpenApiContact
            {
                Name = "Shubaham Saurav",
                Email = "ssaurav28502@gmail.com     ",
                Url = new Uri("https://www.example.com/"),
            },
            License = new OpenApiLicense
            {
                Name = "License",
                Url = new Uri("https://www.example.com/"),
            }
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

        //var filename = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
        //var filepath = Path.Combine(AppContext.BaseDirectory, filename);
        //c.IncludeXmlComments(filepath);
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

        var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    var rabbitConsumer = app.Services.GetRequiredService<RabbitMQConsumer>();
    Task.Run(() => rabbitConsumer.StartListening());

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.LogError(ex, "Application stopped due to an exception.");
    throw;
}
