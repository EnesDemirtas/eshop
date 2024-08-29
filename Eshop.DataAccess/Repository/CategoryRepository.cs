using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	private readonly ApplicationDbContext _context;

	public CategoryRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public void Update(Category category)
	{
		_context.Categories.Update(category);
	}
}