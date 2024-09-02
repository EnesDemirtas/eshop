using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
	private readonly ApplicationDbContext _context;

	public ProductImageRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public void Update(ProductImage image)
	{
		_context.ProductImages.Update(image);
	}
}