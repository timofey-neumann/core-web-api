using Infrastructure.Data;
using Core.Entities.General;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<double> PriceCheck(int productId, CancellationToken cancellationToken = default)
    {
        var price = await _dbContext.Products
            .Where(x => x.Id == productId)
            .Select(x => x.Price)
            .FirstOrDefaultAsync(cancellationToken);
        return price;
    }
}