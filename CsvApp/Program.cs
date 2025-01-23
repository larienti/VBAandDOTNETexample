var builder = WebApplication.CreateBuilder(args);

// Explicitly configure Kestrel with HTTP and HTTPS endpoints
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5053); // HTTP
    options.ListenAnyIP(7121, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});
// Add services to the container.
builder.Services.AddControllersWithViews();

//Add EF Core services and specify the connection string:
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//                         ?? Environment.GetEnvironmentVariable("DATABASE_URL");
//
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(connectionString));
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Csv}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
