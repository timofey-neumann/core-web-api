﻿using Core.Services;
using Core.Interfaces.IServices;
using Infrastructure.Repositories;
using Core.Interfaces.IRepositories;

namespace API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        #region Services

        services.AddSingleton<IUserContext, UserContext>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        #endregion

        #region Repositories

        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAuthRepository, AuthRepository>();

        #endregion

        return services;
    }
}