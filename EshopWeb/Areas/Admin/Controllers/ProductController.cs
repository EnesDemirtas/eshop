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
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return View(objProductList);
    }

    public IActionResult Upsert(int? id)
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
        if (id != null && id != 0)
            productVM.Product = _unitOfWork.Product.Get(p => p.Id == id);
        return View(productVM);
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null) {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images/product");

                if (!string.IsNullOrEmpty(obj.Product.ImageUrl)) {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath)) {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
                    file.CopyTo(fileStream);
                }

                obj.Product.ImageUrl = @"/images/product/" + fileName;
            }
            if (obj.Product.Id == 0) {
                _unitOfWork.Product.Add(obj.Product);
            } else {
                _unitOfWork.Product.Update(obj.Product);
            }
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