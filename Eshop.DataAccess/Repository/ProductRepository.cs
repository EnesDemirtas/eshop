using Eshop.Models;
using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;

namespace Eshop.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository {
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context) {
        _context = context;
    }

    public void Update(Product product) {
        _context.Products.Update(product);
    }
}