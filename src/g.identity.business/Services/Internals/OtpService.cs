using g.commons.Models;
using g.identity.business.Handlers.Client;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using OtpNet;

namespace g.identity.business.Services.Internals;

internal class OtpService : IOtpService
{
    private const OtpHashMode HashMode = OtpHashMode.Sha1;
    private const int DefaultTimeRange = 30;
    private const int DefaultTokenSize = 6;

    private readonly IUnitOfWork _unitOfWork;

    public OtpService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> NotifyWithCode(AppUser user, string jti, CancellationToken token = default)
    {
        var otp = await _unitOfWork.OtpCodes.Read(e => e.Jti == jti, token);
        var newOne = otp == null;

        otp ??= new OtpCode { Jti = jti, AppId = user.AppId, UserId = user.Id };
        if (otp.Updated > DateTime.UtcNow.Add(-TimeSpan.FromSeconds(30)))
            return Result<string>.Bad("Too-o-o-o early");

        otp.HasSecondFactor = user.TwoFactorEnabled;
        otp.Code = GetCode();
        otp.Updated = DateTime.UtcNow;

        var _ = newOne
            ? await _unitOfWork.OtpCodes.AddAndSave(otp, token)
            : await _unitOfWork.OtpCodes.UpdateAndSave(otp, token);

        await SendNotification(user, otp.Code, token);

        return Result<string>.Ok(otp.Code);
    }

    public async Task<Result> ValidateCode(
        AppUser user, string jti, string code, bool has2Fa, CancellationToken token = default)
    {
        var otp = await _unitOfWork.OtpCodes.Read(e => e.Jti == jti, token);
        if (otp is null)
            return Result.Bad("OTP not sent at all");

        if (otp.Validated)
            return await ValidateSecondFactor(user, code, token);

        if (otp.Code != code)
            return Result.Bad("Invalid OTP");

        if (!otp.HasSecondFactor)
        {
            await _unitOfWork.OtpCodes.RemoveAndSave(otp, token);
            return Result.Ok();
        }

        otp.Validated = true;
        otp.Updated = DateTime.UtcNow;
        await _unitOfWork.OtpCodes.UpdateAndSave(otp, token);
        return Result.Ok();
    }

    public async Task<Result<SecondFactorDto>> CreateSecondFactor(AppUser user, CancellationToken token = default)
    {
        var hash = KeyGeneration.GenerateRandomKey();
        var otp = new Totp(hash, DefaultTimeRange, HashMode, DefaultTokenSize);
        var entry = new SecondFactor
        {
            Hash = hash,
            AppId = user.AppId,
            UserId = user.Id,
            IsActive = true,
            TimeToLive = otp.RemainingSeconds(),
            Token = otp.ComputeTotp(),
        };
        
        if (await _unitOfWork.OtpCodes.AddSecondFactor(entry, token))
            return Result<SecondFactorDto>.Ok(new SecondFactorDto
            {
                Hash = Base32Encoding.ToString(hash),
                Token = entry.Token,
                TimeToLive = entry.TimeToLive,
            });

        return Result<SecondFactorDto>.Failed("Could not create second factor");
    }

    private async Task<Result> ValidateSecondFactor(AppUser user, string code, CancellationToken token)
    {
        var factor = await _unitOfWork.OtpCodes.ReadFactor(user.Id, user.AppId, token);
        if (factor is null)
            return Result.NotFound("2FA not found");

        var otp = new Totp(factor.Hash, DefaultTimeRange, HashMode, DefaultTokenSize);
        return otp.VerifyTotp(code, out _) ? Result.Ok() : Result.Bad("2FA not valid");
    }

    private async Task SendNotification(AppUser user, string otpCode, CancellationToken token)
    {
        await Task.Yield();
    }

    private static string GetCode()
        => string.Concat(Guid.NewGuid().ToString("N").Where(char.IsDigit))[..6];
}