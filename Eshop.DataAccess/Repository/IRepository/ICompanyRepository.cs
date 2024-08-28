using Eshop.Models;

namespace Eshop.DataAccess.Repository.IRepository;

public interface ICompanyRepository : IRepository<Company> {
    void Update(Company company);
}