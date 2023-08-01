using g.identity.dataAccess.Repositories;
using g.identity.dataAccess.Repositories.Internals;

namespace g.identity.dataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IdentityAppContext _context;
    private readonly Lazy<IAppRepository> _apps;
    private readonly Lazy<IUsersRepository> _users;
    private readonly Lazy<IRolesRepository> _roles;
    private readonly Lazy<IUserRolesRepository> _userRoles;
    private readonly Lazy<IOtpCodesRepository> _oTps;

    public UnitOfWork(IdentityAppContext context)
    {
        _context = context;
        _apps = new Lazy<IAppRepository>(() => new AppRepository(context));
        _users = new Lazy<IUsersRepository>(() => new UsersRepository(context));
        _roles = new Lazy<IRolesRepository>(() => new RolesRepository(context));
        _userRoles = new Lazy<IUserRolesRepository>(() => new UserRolesRepository(context));
        _oTps = new Lazy<IOtpCodesRepository>(() => new OtpCodesRepository(context));
    }

    public IdentityAppContext Context => _context;
    public IAppRepository Apps => _apps.Value;
    public IUsersRepository Users => _users.Value;
    public IRolesRepository Roles => _roles.Value;
    public IUserRolesRepository UserRoles => _userRoles.Value;
    public IOtpCodesRepository OtpCodes => _oTps.Value;

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0;
}