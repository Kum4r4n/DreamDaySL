using DreamDay;
using DreamDay.Data;
using DreamDay.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<WeddingService>();
builder.Services.AddScoped<ChecklistService>();
builder.Services.AddScoped<BudgetService>();
builder.Services.AddScoped<GuestService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddScoped<CoupleTimelineService>();

builder.Services.AddScoped<PlannerService>();
builder.Services.AddScoped<PlannerChecklistService>();
builder.Services.AddScoped<PlannerNotesService>();
builder.Services.AddScoped<PlannerReportsService>();

builder.Services.AddScoped<AdminService>();


builder.Services.AddDbContext<ApplicaitonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));


builder.Services.AddAuthentication("DreamDay")
    .AddCookie("DreamDay", options =>
    {
        options.LoginPath = "/Login/Index";  
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicaitonDbContext>();
    SeedData.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
