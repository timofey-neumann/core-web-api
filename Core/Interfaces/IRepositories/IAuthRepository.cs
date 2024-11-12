using Core.Entities.Business;

namespace Core.Interfaces.IRepositories;

public interface IAuthRepository
{
    Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);
    Task Logout();
}