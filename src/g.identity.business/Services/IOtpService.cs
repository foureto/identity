using g.commons.Models;
using g.identity.business.Handlers.Client;
using g.identity.dataAccess.Domain;

namespace g.identity.business.Services;

public interface IOtpService
{
    Task<Result<string>> NotifyWithCode(AppUser user, string jti, CancellationToken token = default);
    Task<Result> ValidateCode(AppUser user, string jti, string code, bool has2Fa, CancellationToken token = default);
    Task<Result<SecondFactorDto>> CreateSecondFactor(AppUser user, CancellationToken token = default);
}