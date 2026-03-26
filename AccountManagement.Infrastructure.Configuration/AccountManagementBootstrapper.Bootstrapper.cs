using AccountManagement.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Infrastructure.EFCore;
using AccountManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Infrastructure.Configuration;

public static class AccountManagementBootstrapper
{
    public static void Configure(IServiceCollection service, string connectionString)
    {
        service.AddTransient<IAccountApplication, AccountApplication>();
        service.AddTransient<IAccountRepository, AccountRepository>();

        service.AddTransient<IRoleApplication, RoleApplication>();
        service.AddTransient<IRoleRepository, RoleRepository>();

        service.AddDbContext<AccountContext>(x =>
            x.UseNpgsql(connectionString));
    }
}