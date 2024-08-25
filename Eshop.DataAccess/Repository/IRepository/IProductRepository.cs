using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<Product> {
    void Update(Product product);
}