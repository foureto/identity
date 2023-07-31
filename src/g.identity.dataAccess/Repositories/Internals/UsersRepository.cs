using System.Security.Claims;
using g.commons.Identity;
using g.identity.dataAccess.Domain;
using Microsoft.EntityFrameworkCore;

namespace g.identity.dataAccess.Repositories.Internals;

internal class UsersRepository : BaseRepository<AppUser>, IUsersRepository
{
    private readonly IdentityAppContext _context;

    public UsersRepository(IdentityAppContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Claim>> GetUserClaims(string userId, CancellationToken token = default)
    {

        var claims = await _context.UserClaims
            .Where(e => e.UserId == userId)
            .Select(e => new Claim(e.ClaimType, e.ClaimValue))
            .ToListAsync(token);

        var roles = await _context.UserRoles
            .Where(e => e.UserId == userId)
            .Select(e => new Claim(ClaimTypes.Role, e.Role.Name))
            .ToListAsync(token);

        claims.AddRange(roles);

        return claims;
    }
}