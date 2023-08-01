using System.Security.Claims;
using g.commons.Extensions;
using g.commons.Identity;
using g.commons.Models;
using g.identity.dataAccess;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;

namespace g.identity.business.Services.Internals;

internal class SessionProvider : ISessionProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<AppSettings> _options;

    public SessionProvider(IUnitOfWork unitOfWork, IOptions<AppSettings> options)
    {
        _unitOfWork = unitOfWork;
        _options = options;
    }

    public Task<Result> ValidateToken(string token, bool temp)
    {
        try
        {
#pragma warning disable CS0618
            JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(temp ? _options.Value.TempKey : _options.Value.CommonKey)
                .Decode(token);
#pragma warning restore CS0618
            
            return Task.FromResult(Result.Ok());
        }
        catch (Exception e)
        {
            return Task.FromResult(Result.Bad("Invalid token"));
        }
    }

    public async Task<Result<(string jwt, string refresh, string jti)>> NewToken(string userId, bool temp)
    {
        var user = await _unitOfWork.Users.Read(
            e => e.Id == userId, e => new { e.Id, e.Nickname, e.AppId, e.Email });
        if (user is null)
            return Result<(string jwt, string refresh, string jti)>.NotFound("User not found");

        var userClaims = await _unitOfWork.Users.GetUserClaims(userId);

        var jti = Guid.NewGuid().ToString("N");
        userClaims.Add(new Claim(AppClaims.UserId, user.Id));
        userClaims.Add(new Claim(AppClaims.AppId, user.AppId));
        userClaims.Add(new Claim(AppClaims.Jti, jti));
        userClaims.Add(new Claim(AppClaims.Iat, DateTime.UtcNow.ToString("O")));
        userClaims.Add(new Claim(AppClaims.NameId, user.Nickname ?? user.Email));

#pragma warning disable CS0618
        var jwtBuilder = JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm());
#pragma warning restore CS0618

        userClaims.ForEach(c => jwtBuilder.AddClaim(c.Type, c.Value));
        jwtBuilder.WithSecret(temp ? _options.Value.TempKey : _options.Value.CommonKey);

        return Result<(string jwt, string refresh, string jti)>.Ok((jwtBuilder.Encode(), jti.Sha512(), jti));
    }
}