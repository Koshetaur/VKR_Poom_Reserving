using DomainLayer;
using LibBase;
using Microsoft.EntityFrameworkCore;
using VKR_Poom_Reserving;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationContext>>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddMediatR(cfg => {cfg.RegisterServicesFromAssembly(typeof(AddReserveCommand).Assembly);});

builder.Services
    .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
