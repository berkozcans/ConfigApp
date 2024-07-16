using System.Diagnostics;
using ConfigurationLibrary;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAppConfig.Models;

namespace WebAppConfig.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Configuration> _configurations;

    public HomeController(ILogger<HomeController> logger, IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase("ConfigurationDb");
        _configurations = _database.GetCollection<Configuration>("Configurations");
        _logger = logger;
    }

    public IActionResult Index()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("ConfigurationDb");
        var collection = database.GetCollection<Configuration>("Configurations");
        var filter = Builders<Configuration>.Filter.And(
            Builders<Configuration>.Filter.Eq(c => c.ApplicationName, "SERVICE-A"),
            Builders<Configuration>.Filter.Eq(c => c.IsActive, true)
        );
        
        var data = collection.Find(filter).ToList();
        if (data == null || data.Count == 0)
        {
            // Veri bulunamadı, bir hata mesajı veya varsayılan bir değer gönderilebilir.
            ViewBag.ErrorMessage = "Konfigürasyon verisi bulunamadı.";
        }
        return View(data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Configuration configuration)
    {
        if (ModelState.IsValid)
        {
            _configurations.InsertOne(configuration);
            return RedirectToAction("Index");
        }
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return View(configuration);
    }
    public IActionResult Edit(string id)
    {
        var objectId = new ObjectId(id);
        var configuration = _configurations.Find(c => c._id == objectId).FirstOrDefault();
    
        if (configuration == null)
        {
            return NotFound();
        }
    
        return View(configuration);
    }

    [HttpPost]
    public IActionResult Edit(string id, Configuration updatedConfiguration)
    {
        var objectId = new ObjectId(id); // id stringini ObjectId türüne dönüştür

        if (objectId != updatedConfiguration._id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _configurations.ReplaceOne(c => c._id == objectId, updatedConfiguration);
            return RedirectToAction("Index");
        }

        return View(updatedConfiguration);
    }


}