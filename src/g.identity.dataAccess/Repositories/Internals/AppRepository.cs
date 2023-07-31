using g.identity.dataAccess.Domain;
using Microsoft.EntityFrameworkCore;

namespace g.identity.dataAccess.Repositories.Internals;

internal class AppRepository : BaseRepository<Application>, IAppRepository
{
    public AppRepository(DbContext context) : base(context)
    {
    }
}