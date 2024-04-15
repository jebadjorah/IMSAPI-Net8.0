using IMSAPI.DB;
using IMSAPI.Installer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var identitySettingsSection = builder.Configuration.GetSection("AppSettings");
var AuthentiationType = builder.Configuration.GetSection("AuthentiationType").Value;

InstallerExtenstions.IntallServices(builder, identitySettingsSection);

builder.Services.AddLogging();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

if (AuthentiationType == "2")
{
    builder.Services.AddAuthentication(setup =>
    {
        setup.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        setup.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        setup.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(key: "AzureAd"));
}
else
{
    builder.Services.AddAuthentication(setup =>
    {
        setup.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        setup.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        setup.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8
                .GetBytes(builder.Configuration["JwtTokenSettings:SymmetricSecurityKey"])
            ),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtTokenSettings:ValidAudience"],
            ValidIssuer = builder.Configuration["JwtTokenSettings:ValidIssuer"],
            ClockSkew = TimeSpan.Zero

        };
    });
}



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
     throw new InvalidOperationException("Connection String is not found"));
});


//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder =>
        builder
        .WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader());

app.MapControllers();

app.Run();
