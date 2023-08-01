using g.identity.dataAccess.Domain;

namespace g.identity.dataAccess.Repositories.Internals;

internal class UserRolesRepository : BaseRepository<AppUserRole>, IUserRolesRepository
{
    public UserRolesRepository(IdentityAppContext context) : base(context)
    {
    }
}