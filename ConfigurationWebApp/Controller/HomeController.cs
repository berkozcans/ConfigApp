using Microsoft.AspNetCore.Mvc;

namespace ConfigurationWebApp;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}