﻿using Core.Entities.Business;

namespace Core.Interfaces.IServices;

public interface IRoleService : IBaseService<RoleViewModel>
{
    Task<RoleViewModel> Create(RoleCreateViewModel model, CancellationToken cancellationToken);
    Task Update(RoleUpdateViewModel model, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);
}