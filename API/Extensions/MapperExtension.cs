using AutoMapper;
using Core.Mapper;
using Core.Entities.General;
using Core.Entities.Business;
using Core.Interfaces.IMapper;

namespace API.Extensions;

public static class MapperExtension
{
    public static IServiceCollection RegisterMapperService(this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(sp => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductViewModel>();
            cfg.CreateMap<ProductCreateViewModel, Product>();
            cfg.CreateMap<ProductUpdateViewModel, Product>();

            cfg.CreateMap<Role, RoleViewModel>();
            cfg.CreateMap<RoleCreateViewModel, Role>();
            cfg.CreateMap<RoleUpdateViewModel, Role>();

            cfg.CreateMap<User, UserViewModel>();
            cfg.CreateMap<UserViewModel, User>();

        }).CreateMapper());

        services.AddSingleton<IBaseMapper<Product, ProductViewModel>, BaseMapper<Product, ProductViewModel>>();
        services.AddSingleton<IBaseMapper<ProductCreateViewModel, Product>, BaseMapper<ProductCreateViewModel, Product>>();
        services.AddSingleton<IBaseMapper<ProductUpdateViewModel, Product>, BaseMapper<ProductUpdateViewModel, Product>>();

        services.AddSingleton<IBaseMapper<Role, RoleViewModel>, BaseMapper<Role, RoleViewModel>>();
        services.AddSingleton<IBaseMapper<RoleCreateViewModel, Role>, BaseMapper<RoleCreateViewModel, Role>>();
        services.AddSingleton<IBaseMapper<RoleUpdateViewModel, Role>, BaseMapper<RoleUpdateViewModel, Role>>();

        services.AddSingleton<IBaseMapper<User, UserViewModel>, BaseMapper<User, UserViewModel>>();
        services.AddSingleton<IBaseMapper<UserViewModel, User>, BaseMapper<UserViewModel, User>>();

        return services;
    }
}