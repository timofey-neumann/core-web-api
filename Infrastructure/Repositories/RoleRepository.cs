using Infrastructure.Data;
using Core.Entities.General;
using Core.Interfaces.IRepositories;

namespace Infrastructure.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}
