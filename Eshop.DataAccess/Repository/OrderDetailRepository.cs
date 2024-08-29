using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;

namespace Eshop.DataAccess.Repository;

public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
{
	private readonly ApplicationDbContext _context;

	public OrderDetailRepository(ApplicationDbContext context) : base(context)
	{
		_context = context;
	}

	public void Update(OrderDetail orderDetail)
	{
		_context.OrderDetails.Update(orderDetail);
	}
}