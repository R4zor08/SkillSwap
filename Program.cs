using Microsoft.EntityFrameworkCore;
using SkillSwap.Data;
using SkillSwap.Services;
using SkillSwap.Services.Interfaces;
using SkillSwap.Web.Requests;
using SkillSwap.Web.Requests.Interfaces;
using SkillSwap.Web.Requests.Handlers;

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

app.Run();
