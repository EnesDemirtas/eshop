using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EshopWeb.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartVM ShoppingCartVM { get; set; }
    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == userId,
            includeProperties: "Product")
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderTotal += cart.Price * cart.Count;
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        return View();
    }

    public IActionResult Plus(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.Id == cartId);
        cartFromDb.Count += 1;
        _unitOfWork.ShoppingCart.Update(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

    public IActionResult Minus(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.Id == cartId);
        if (cartFromDb.Count <= 1)
        {
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
        }
        else
        {
            cartFromDb.Count -= 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }

        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

    public IActionResult Remove(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.Id == cartId);
        _unitOfWork.ShoppingCart.Remove(cartFromDb);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Count <= 50)
        {
            return shoppingCart.Product.Price;
        }
        else if (shoppingCart.Count <= 100)
        {
            return shoppingCart.Product.Price50;
        }
        else
        {
            return shoppingCart.Product.Price100;
        }
    }
}
