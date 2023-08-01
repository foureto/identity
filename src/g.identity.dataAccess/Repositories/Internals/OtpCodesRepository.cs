using g.identity.dataAccess.Domain;
using Microsoft.EntityFrameworkCore;

namespace g.identity.dataAccess.Repositories.Internals;

internal class OtpCodesRepository : BaseRepository<OtpCode>, IOtpCodesRepository
{
    private readonly IdentityAppContext _context;

    public OtpCodesRepository(IdentityAppContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> AddSecondFactor(SecondFactor secondFactor, CancellationToken token = default)
    {
        _context.SecondFactors.Add(secondFactor);
        return await _context.SaveChangesAsync(token) > 0;
    }

    public Task<SecondFactor> ReadFactor(string userId, string appId, CancellationToken token = default)
        => _context.SecondFactors.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId && e.AppId == appId, token);

    public void RemoveFactor(SecondFactor factor)
    {
        _context.SecondFactors.Remove(factor);
    }

    public void CreateFactor(SecondFactor newFactor)
    {
        _context.SecondFactors.Add(newFactor);
    }
}