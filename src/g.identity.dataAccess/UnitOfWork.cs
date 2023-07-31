using g.identity.dataAccess.Repositories;
using g.identity.dataAccess.Repositories.Internals;

namespace g.identity.dataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IdentityAppContext _context;
    private readonly Lazy<IAppRepository> _apps;
    private readonly Lazy<IUsersRepository> _users;

    public UnitOfWork(IdentityAppContext context)
    {
        _context = context;
        _apps = new Lazy<IAppRepository>(() => new AppRepository(context));
        _users = new Lazy<IUsersRepository>(() => new UsersRepository(context));
    }

    public IdentityAppContext Context => _context;
    public IAppRepository Apps => _apps.Value;
    public IUsersRepository Users => _users.Value;
}