using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eshop.Models.ViewModels;

public class ProductVM
{
	public Product Product { get; set; }
	[ValidateNever]
	public IEnumerable<SelectListItem> CategoryList { get; set; }
}