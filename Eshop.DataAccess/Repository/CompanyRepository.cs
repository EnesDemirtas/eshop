using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
	private readonly ApplicationDbContext _context;

	public CompanyRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public void Update(Company company)
	{
		_context.Companies.Update(company);
	}
}