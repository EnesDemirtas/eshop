using Eshop.Models;
using Eshop.Models.ViewModels;
using Eshop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EshopWeb.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
        return View(objProductList);
    }

    public IActionResult Create()
    {
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                                                    .Select(c => new SelectListItem {
                                                        Text = c.Name,
                                                        Value = c.Id.ToString()
                                                    });
        ProductVM productVM = new () {
            Product = new Product(),
            CategoryList = CategoryList
        };
        return View(productVM);
    }

    [HttpPost]
    public IActionResult Create(ProductVM obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product has been created successfully";
            return RedirectToAction("Index");
        } else {
            obj.CategoryList = _unitOfWork.Category.GetAll()
                                .Select(c => new SelectListItem {
                                    Text = c.Name,
                                    Value = c.Id.ToString()
                                });
            return View(obj);
        }
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Product? product = _unitOfWork.Product.Get(u => u.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product has been updated successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Product? product = _unitOfWork.Product.Get(u => u.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        if (obj == null) return NotFound();
        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product has been deleted successfully";
        return RedirectToAction("Index");
    }
}