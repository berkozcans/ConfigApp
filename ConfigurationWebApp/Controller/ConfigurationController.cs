using ConfigurationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationWebApp;

public class ConfigurationController : Controller
{
    private readonly ConfigurationReader _configurationReader;

    public ConfigurationController(ConfigurationReader configurationReader)
    {
        _configurationReader = configurationReader;
    }

    public IActionResult Index()
    {
        var activeConfigurations = _configurationReader.GetActiveConfigurations();
        return View(activeConfigurations);
    }

    // Ekstra aksiyonlar: Create, Edit, Delete için eklenebilir.
}