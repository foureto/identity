using g.identity.dataAccess.Repositories;

namespace g.identity.dataAccess;

public interface IUnitOfWork
{
    IdentityAppContext Context { get; }
    
    IAppRepository Apps { get; }
    IUsersRepository Users { get; }
    IRolesRepository Roles { get; }
    IUserRolesRepository UserRoles { get; }
    IOtpCodesRepository OtpCodes { get; }

    Task<bool> Save();
}