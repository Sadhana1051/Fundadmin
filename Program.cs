using System.Reflection;
using System.Text;
using FundAdministration.Api.Data;
using FundAdministration.Api.Entities;
using FundAdministration.Api.Middleware;
using FundAdministration.Api.Repositories;
using FundAdministration.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// DbContext
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("FundDb"));

// Repositories & Services
builder.Services.AddScoped<IFundRepository, FundRepository>();
builder.Services.AddScoped<IInvestorRepository, InvestorRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IFundService, FundService>();
builder.Services.AddScoped<IInvestorService, InvestorService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// JWT (mocked for development - in production use proper issuer/audience/key from config)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "FundAdminApi",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "FundAdminClients",
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "DevelopmentKey_MustBeAtLeast32Characters!")),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
});

// Controllers
builder.Services.AddControllers();

// Swagger with XML comments
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fund Administration API",
        Version = "v1",
        Description = "API for managing investment funds, investors, and transactions."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        opt.IncludeXmlComments(xmlPath);
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

// Health check
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("database");

var app = builder.Build();

// Seed data for development
await SeedDataAsync(app.Services);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Fund Administration API v1"));

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

static async Task SeedDataAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (await db.Funds.AnyAsync()) return;

    var fund = new Fund
    {
        FundId = Guid.NewGuid(),
        Name = "Global Growth Fund",
        Currency = "USD",
        LaunchDate = new DateTime(2020, 1, 1)
    };
    db.Funds.Add(fund);

    var investor = new Investor
    {
        InvestorId = Guid.NewGuid(),
        FullName = "Jane Doe",
        Email = "jane@example.com",
        FundId = fund.FundId
    };
    db.Investors.Add(investor);

    await db.SaveChangesAsync();
}
