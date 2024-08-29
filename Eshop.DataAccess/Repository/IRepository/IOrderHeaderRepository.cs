using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
	void Update(OrderHeader orderHeader);
}