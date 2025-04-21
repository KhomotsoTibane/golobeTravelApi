using FluentValidation;
using GolobeTravelApi;
using GolobeTravelApi.Data;
using GolobeTravelApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Resend;
using Testcontainers.PostgreSql;


//var postgreSqlContainer = new PostgreSqlBuilder().Build();
//await postgreSqlContainer.StartAsync();

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;

PostgreSqlContainer? postgreSqlContainer = null;

if (env == "Development" || env == "Testing")
{
    postgreSqlContainer = new PostgreSqlBuilder().Build();
    await postgreSqlContainer.StartAsync();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(postgreSqlContainer.GetConnectionString());
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
}
else
{
    // Use RDS or other prod DB
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        var conn = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseNpgsql(conn);
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "GolobeTravelApi.xml"));
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "GolobeTravel API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
           Array.Empty<string>()
        }
    });

});

builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

builder.Services.AddSingleton<ISystemClock, SystemClock>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    var conn = postgreSqlContainer.GetConnectionString();
//    options.UseNpgsql(conn);
//    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//    //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend", policy =>
//    {
//        policy.WithOrigins("http://localhost:3000")
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddSingleton<ImageService>();

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(
    builder.Configuration.GetSection("Resend")
);

builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddScoped<EmailService, EmailService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtBearerConfigOptions>();
builder.Services.AddAuthorization();



var app = builder.Build();

//app.Lifetime.ApplicationStopping.Register(() => postgreSqlContainer.DisposeAsync());
if (postgreSqlContainer is not null)
{
    app.Lifetime.ApplicationStopping.Register(() =>
    {
        postgreSqlContainer.DisposeAsync().AsTask().Wait();
    });
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.MigrateAndSeed(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

//app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();


app.Run();

public partial class Program { }

