using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RnD.Angular.Backend.Contracts;
using RnD.Angular.Backend.Models;
using RnD.Angular.Backend.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("api", new OpenApiInfo()
    {
        Description = "Angular Backend WEB API",
        Title = "Customer",
        Version = "v1",
    });
});

builder.Services.AddDbContext<LearnDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.Configure<JWTConfigModel>(builder.Configuration.GetSection("JWTConfig"));

var dbContext = builder.Services.BuildServiceProvider().GetService<LearnDbContext>();
builder.Services.AddSingleton<IRefreshToken>(provider => new RefreshTokenRepositories(dbContext));

var authKey = builder.Configuration.GetValue<string>("JWTConfig:SecurityKey");

builder.Services.AddAuthentication(items =>
{
    items.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    items.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(items =>
{
    items.RequireHttpsMetadata = true;
    items.SaveToken = true;
    items.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("api/swagger.json", "Customer"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseSwaggerUI(options => options.SwaggerEndpoint("api/swagger.json", "Customer"));

app.MapControllers();

app.Run();
