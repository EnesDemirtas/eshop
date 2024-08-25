namespace Eshop.DataAccess.Repository.IRepository;

public interface IUnitOfWork {
    ICategoryRepository Category {get;}

    void Save();
}