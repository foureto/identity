using g.commons.Models;

namespace g.identity.business.Services;

public interface ISessionProvider
{
    Task<Result> ValidateToken(string token, bool temp);
    Task<Result<(string jwt, string refresh)>> NewToken(string userId, bool temp);
}