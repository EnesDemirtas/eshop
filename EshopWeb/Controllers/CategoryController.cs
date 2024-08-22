using Microsoft.AspNetCore.Mvc;

namespace EshopWeb.Controllers;

public class CategoryController : Controller {
    public IActionResult Index() {
        return View();
    }   
}