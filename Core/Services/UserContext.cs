using Core.Interfaces.IServices;

namespace Core.Services;

public class UserContext : IUserContext
{
    public required string UserId { get; set; }
}