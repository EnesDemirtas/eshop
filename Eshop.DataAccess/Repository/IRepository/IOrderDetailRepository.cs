using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
	void Update(OrderDetail orderDetail);
}