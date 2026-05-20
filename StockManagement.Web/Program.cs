using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Web.Middleware;
using StockManagement.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var useSqlAuth = builder.Configuration.GetValue<bool>("Database:UseSqlAuthentication");
var connectionString = useSqlAuth
    ? builder.Configuration.GetConnectionString("SqlAuth")
    : builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Connection string bulunamadı.");

builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddSingleton<DatabaseHealthService>();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

app.UseMiddleware<DatabaseExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
