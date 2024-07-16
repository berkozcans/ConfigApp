using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// MongoDB Configuration
app.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var mongoUrl = new MongoUrl("mongodb://localhost:27017");
    return new MongoClient(mongoUrl);
});

// Services
app.Services.AddControllersWithViews();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
