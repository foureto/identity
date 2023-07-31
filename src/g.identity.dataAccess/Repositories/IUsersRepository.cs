using System.Security.Claims;
using g.identity.dataAccess.Domain;

namespace g.identity.dataAccess.Repositories;

public interface IUsersRepository : IBaseRepository<AppUser>
{
    Task<List<Claim>> GetUserClaims(string userId, CancellationToken token = default);
}