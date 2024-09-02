using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Models.ViewModels;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EshopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
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
													.Select(c => new SelectListItem
													{
														Text = c.Name,
														Value = c.Id.ToString()
													});
		ProductVM productVM = new()
		{
			Product = new Product(),
			CategoryList = CategoryList
		};
		if (id != null && id != 0)
			productVM.Product = _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "ProductImages");
		return View(productVM);
	}

	[HttpPost]
	public IActionResult Upsert(ProductVM obj, List<IFormFile> files)
	{
		if (ModelState.IsValid)
		{
            if (obj.Product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.Product);
            }
            else
            {
                _unitOfWork.Product.Update(obj.Product);
            }
            _unitOfWork.Save();

            string wwwRootPath = _webHostEnvironment.WebRootPath;
			if (files != null)
			{
				foreach(IFormFile file in files)
				{
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = @"images\products\product-" + obj.Product.Id;
                    string finalPath = Path.Combine(wwwRootPath, productPath);

					if (!Directory.Exists(finalPath))
					{
						Directory.CreateDirectory(finalPath);
					}

					using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					ProductImage productImage = new()
					{
						ImageUrl = @"\" + productPath + @"\" + fileName,
						ProductId = obj.Product.Id,
					};

					if (obj.Product.Images == null)
					{
						obj.Product.Images = new List<ProductImage>();
					}

					obj.Product.Images.Add(productImage);
				}

                _unitOfWork.Product.Update(obj.Product);
				_unitOfWork.Save();
            }

            TempData["success"] = "Product has been created/updated successfully";
			return RedirectToAction("Index");
		}
		else
		{
			obj.CategoryList = _unitOfWork.Category.GetAll()
								.Select(c => new SelectListItem
								{
									Text = c.Name,
									Value = c.Id.ToString()
								});
			return View(obj);
		}
	}

	public IActionResult DeleteImage(int imageId)
	{
		var image = _unitOfWork.ProductImage.Get(i => i.Id == imageId);
		int productId = image.ProductId;
		if (image != null)
		{
			if (!string.IsNullOrEmpty(image.ImageUrl))
			{
				var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
				if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
			}

			_unitOfWork.ProductImage.Remove(image);
			_unitOfWork.Save();

			TempData["Success"] = "Deleted successfully";
        }

		return RedirectToAction(nameof(Upsert), new { id = productId });
	}

	#region API CALLS

	[HttpGet]
	public IActionResult GetAll()
	{
		List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
		return Json(new { data = objProductList });
	}

	[HttpDelete]
	public IActionResult Delete(int id)
	{
		var product = _unitOfWork.Product.Get(p => p.Id == id);
		if (product == null) return Json(new { success = false, message = "Error while deleting" });

        string productPath = @"images\products\product-" + id;
        string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

        if (Directory.Exists(finalPath))
        {
			string[] filePaths = Directory.GetFiles(finalPath);
			foreach (string filePath in filePaths)
			{
				System.IO.File.Delete(filePath);
			}
            Directory.Delete(finalPath);
        }

        _unitOfWork.Product.Remove(product);
		_unitOfWork.Save();

		return Json(new { success = true, message = "Delete successful" });
	}

	#endregion
}