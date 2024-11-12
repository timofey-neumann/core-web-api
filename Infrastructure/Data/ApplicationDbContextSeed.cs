﻿using Bogus;
using Core.Entities.General;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, int? retry = 0)
    {
        int retryForAvailability = retry ?? 0;
        var appContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

        try
        {
            if (!appContext.Roles.Any())
            {
                using var transaction = appContext.Database.BeginTransaction();
                appContext.Roles.AddRange(Roles());

                await appContext.SaveChangesAsync();
                transaction.Commit();
            }

            if (!appContext.Users.Any())
            {
                var defaultUser = new User { FullName = "Kawser Hamid", UserName = "hamid", RoleId = 1, Email = "kawser2133@gmail.com", EntryDate = DateTime.Now, IsActive = true };
                IdentityResult userResult = await UserManager.CreateAsync(defaultUser, "Hamid@12");
                if (userResult.Succeeded)
                {
                    await UserManager.AddToRoleAsync(defaultUser, "ADMIN");
                }
            }

            if (!appContext.Products.Any())
            {
                using var transaction = appContext.Database.BeginTransaction();
                appContext.Products.AddRange(Products());

                await appContext.SaveChangesAsync();
                transaction.Commit();
            }

        }
        catch (Exception ex)
        {
            if (retryForAvailability < 10)
            {
                retryForAvailability++;
                var log = loggerFactory.CreateLogger<ApplicationDbContextSeed>();

                log.LogError(ex.Message);
                await SeedAsync(serviceProvider, loggerFactory, retryForAvailability);
            }
        }
    }

    static IEnumerable<Role> Roles()
    {
        return new List<Role>
            {
                new Role {Code="ADMIN", Name = "Admin", NormalizedName="ADMIN", IsActive = true, EntryDate= DateTime.Now },
                new Role {Code="USER", Name = "User", NormalizedName= "USER", IsActive = true, EntryDate= DateTime.Now },
            };
    }

    static IEnumerable<Product> Products()
    {
        var faker = new Faker<Product>()
            .RuleFor(c => c.Code, f => f.Commerce.Product())
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
            .RuleFor(c => c.Price, f => Convert.ToDouble(f.Commerce.Price(1, 1000, 0)))
            .RuleFor(c => c.Quantity, f => f.Commerce.Random.Number(100))
            .RuleFor(c => c.IsActive, f => f.Random.Bool())
            .RuleFor(c => c.EntryDate, DateTime.Now);

        return faker.Generate(100);
    }
}
