using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EshopWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IUnitOfWork _unitOfWork;

	public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
		return View(productList);
	}

	public IActionResult Details(int id)
	{
		ShoppingCart cart = new()
		{
			Product = _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Category,ProductImages"),
			Count = 1,
			ProductId = id
		};
		return View(cart);
	}

	[HttpPost]
	[Authorize]
	public IActionResult Details(ShoppingCart cart)
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
		cart.ApplicationUserId = userId;

		ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.
			Get(sc => sc.ApplicationUserId == userId
				&& sc.ProductId == cart.ProductId);

		if (cartFromDb != null)
		{
			cartFromDb.Count += cart.Count;
			_unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
        }
        else
		{
			_unitOfWork.ShoppingCart.Add(cart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(StaticDetails.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == userId).Count());
		}
		TempData["success"] = "Cart updated successfully";

		return RedirectToAction(nameof(Index));
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
}
