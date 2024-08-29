using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
	void Update(ShoppingCart shoppingCart);
}