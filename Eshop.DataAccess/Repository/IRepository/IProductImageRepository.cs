using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface IProductImageRepository : IRepository<ProductImage>
{
	void Update(ProductImage image);
}