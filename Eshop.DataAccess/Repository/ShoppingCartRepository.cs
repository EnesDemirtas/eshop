using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ShoppingCart shoppingCart)
    {
        _context.ShoppingCarts.Update(shoppingCart);
    }
}