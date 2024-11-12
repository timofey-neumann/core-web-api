using Core.Entities.General;

namespace Core.Interfaces.IRepositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<double> PriceCheck(int productId, CancellationToken cancellationToken);
}