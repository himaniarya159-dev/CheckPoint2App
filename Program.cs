using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CheckPoint2App.Data;
using CheckPoint2App.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register SQLite Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 1. ADD COOKIE AUTHENTICATION SERVICE ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
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

// --- 2. ENABLE AUTHENTICATION MIDDLEWARE ---
// Must come after UseRouting() and before UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

// --- DATABASE INITIALIZATION BLOCK ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Creates the database file and tables if they do not exist
    context.Database.EnsureCreated();

    // Check and seed students safely
    if (!context.Students.Any())
    {
        context.Students.AddRange(
            new Student { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Course = "Computer Science" },
            new Student { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Course = "Information Technology" },
            new Student { FirstName = "Alex", LastName = "Johnson", Email = "alex.j@example.com", Course = "Software Engineering" },
            new Student { FirstName = "Emily", LastName = "Davis", Email = "emily.davis@example.com", Course = "Data Science" },
            new Student { FirstName = "Michael", LastName = "Brown", Email = "michael.b@example.com", Course = "Cybersecurity" },
            new Student { FirstName = "Sarah", LastName = "Wilson", Email = "sarah.w@example.com", Course = "Web Development" },
            new Student { FirstName = "David", LastName = "Taylor", Email = "david.t@example.com", Course = "Database Administration" },
            new Student { FirstName = "Jessica", LastName = "Anderson", Email = "jessica.a@example.com", Course = "Cloud Computing" },
            new Student { FirstName = "Daniel", LastName = "Martinez", Email = "daniel.m@example.com", Course = "Network Engineering" },
            new Student { FirstName = "Amanda", LastName = "Thomas", Email = "amanda.t@example.com", Course = "Artificial Intelligence" }
        );
        context.SaveChanges();
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();