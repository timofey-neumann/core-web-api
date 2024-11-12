using Core.Entities.General;
using Core.Entities.Business;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.IRepositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IdentityResult> Create(UserCreateViewModel model);
    Task<IdentityResult> Update(UserUpdateViewModel model);
    Task<IdentityResult> ResetPassword(ResetPasswordViewModel model);
}