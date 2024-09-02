using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
	private readonly ApplicationDbContext _context;

	public ApplicationUserRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

    public void Update(ApplicationUser applicationUser)
    {
		_context.ApplicationUsers.Update(applicationUser);
    }
}