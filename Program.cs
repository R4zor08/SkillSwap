using Microsoft.EntityFrameworkCore;
using SkillSwap.Data;
using SkillSwap.Services;
using SkillSwap.Services.Interfaces;
using SkillSwap.Web.Requests;
using SkillSwap.Web.Requests.Interfaces;
using SkillSwap.Web.Requests.Handlers;
using SkillSwap.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITalentService, TalentService>();
builder.Services.AddScoped<ITradeService, TradeService>();
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
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
