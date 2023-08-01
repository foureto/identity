using g.identity.dataAccess.Domain;

namespace g.identity.dataAccess.Repositories.Internals;

internal class RolesRepository : BaseRepository<AppRole>, IRolesRepository
{
    public RolesRepository(IdentityAppContext context) : base(context)
    {
    }
}