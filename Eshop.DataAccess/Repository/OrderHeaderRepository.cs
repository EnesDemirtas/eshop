using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
	private readonly ApplicationDbContext _context;

	public OrderHeaderRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public void Update(OrderHeader orderHeader)
	{
		_context.OrderHeaders.Update(orderHeader);
	}
}