using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category> {
    void Update(Category category);
}