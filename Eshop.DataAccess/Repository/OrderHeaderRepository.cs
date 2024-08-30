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

	public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
	{
		var orderFromDb = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
		if (orderFromDb != null)
		{
			orderFromDb.OrderStatus = orderStatus;
			if (!string.IsNullOrEmpty(paymentStatus))
			{
				orderFromDb.PaymentStatus = paymentStatus;
			}
		}
	}

	public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
	{
		var orderFromDb = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
		if (!string.IsNullOrEmpty(sessionId))
		{
			orderFromDb.SessionId = sessionId;
		}

		if (!string.IsNullOrEmpty(paymentIntentId))
		{
			orderFromDb.PaymentIntentId = paymentIntentId;
			orderFromDb.PaymentDate = DateTime.Now;
		}
	}
}