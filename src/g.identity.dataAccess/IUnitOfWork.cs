using g.identity.dataAccess.Repositories;

namespace g.identity.dataAccess;

public interface IUnitOfWork
{
    IdentityAppContext Context { get; }
    
    IAppRepository Apps { get; }
    IUsersRepository Users { get; }
}