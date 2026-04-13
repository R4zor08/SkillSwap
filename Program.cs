using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillSwap.Data;
using SkillSwap.Data.Entities;
using SkillSwap.Services;
using SkillSwap.Services.Interfaces;
using SkillSwap.Web.Options;
using SkillSwap.Web.Requests;
using SkillSwap.Web.Requests.Handlers;
using SkillSwap.Web.Requests.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Single URL avoids binding 127.0.0.1:5000 twice (localhost often resolves to the same address → "address already in use").
if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
    builder.WebHost.UseUrls("http://localhost:5000");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException($"Configuration section '{JwtSettings.SectionName}' is missing.");
if (string.IsNullOrWhiteSpace(jwtSettings.Secret) || jwtSettings.Secret.Length < 32)
    throw new InvalidOperationException("Jwt:Secret must be at least 32 characters.");

var jwtSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = jwtSigningKey,
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});

// Configure Entity Framework (relative SQLite path = next to project root, not another machine's folder)
var sqliteConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
var sqliteCsb = new SqliteConnectionStringBuilder(sqliteConnectionString);
if (!Path.IsPathRooted(sqliteCsb.DataSource))
    sqliteCsb.DataSource = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, sqliteCsb.DataSource));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(sqliteCsb.ConnectionString));

// Register application services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITalentService, TalentService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddSingleton<ISmsService, SmsService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

// Register Request Pattern services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<StudentRequestHandler>();
builder.Services.AddScoped<TalentRequestHandler>();
builder.Services.AddScoped<TradeRequestHandler>();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.Students.Any())
    {
        var student1 = new StudentEntity { StudentId = Guid.NewGuid(), Name = "Alice Johnson", Email = "alice@example.com", Password = "password123" };
        var student2 = new StudentEntity { StudentId = Guid.NewGuid(), Name = "Bob Smith", Email = "bob@example.com", Password = "password123" };
        dbContext.Students.AddRange(student1, student2);
        dbContext.SaveChanges();

        var talent1 = new TalentEntity { TalentId = Guid.NewGuid(), TalentName = "Guitar Playing", Description = "Expert in acoustic guitar", StudentId = student1.StudentId, ProficiencyLevel = 5 };
        var talent2 = new TalentEntity { TalentId = Guid.NewGuid(), TalentName = "Cooking", Description = "Italian cuisine specialist", StudentId = student1.StudentId, ProficiencyLevel = 4 };
        var talent3 = new TalentEntity { TalentId = Guid.NewGuid(), TalentName = "Programming", Description = "C# developer", StudentId = student2.StudentId, ProficiencyLevel = 3 };
        dbContext.Talents.AddRange(talent1, talent2, talent3);
        dbContext.SaveChanges();

        var trade = new TradeRequestEntity
        {
            TradeId = Guid.NewGuid(),
            RequesterId = student1.StudentId,
            TargetStudentId = student2.StudentId,
            RequestedTalentId = talent3.TalentId,
            OfferedTalentId = talent1.TalentId,
            Status = 0, // Assuming 0 is pending
            RequestedAt = DateTime.UtcNow,
            Message = "I'd like to learn programming in exchange for guitar lessons."
        };
        dbContext.TradeRequests.Add(trade);
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Avoid redirecting http://localhost:5000 API calls to HTTPS when no HTTPS URL is configured (common in Postman/local dev).
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.MapControllers();

app.Run();
