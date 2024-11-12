using Core.Entities.Business;

namespace Core.Interfaces.IServices;

public interface IAuthService
{
    Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);
    Task Logout();
}