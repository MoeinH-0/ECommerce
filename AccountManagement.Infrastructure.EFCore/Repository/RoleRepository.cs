using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository;

public class RoleRepository : RepositoryBase<long, Role>, IRoleRepository
{
    private readonly AccountContext _accountContext;

    public RoleRepository(AccountContext accountContext) : base(accountContext)
    {
        _accountContext = accountContext;
    }

    public List<RoleViewModel> List()
    {
        return _accountContext.Roles
            .Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
    }

    public EditRole? GetDetails(long id)
    {
        return _accountContext.Roles
            .Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MapPermissions(x.Permissions)
            }).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
    }

    private static List<PermissionDto> MapPermissions(IEnumerable<Permission> permissions)
    {
        return permissions.Select(x =>
            new PermissionDto(x.Code, x.Name)).ToList();
    }
}