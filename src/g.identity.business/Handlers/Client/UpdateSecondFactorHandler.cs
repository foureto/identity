using g.commons.Models;
using g.identity.business.Services;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;
using OtpNet;

namespace g.identity.business.Handlers.Client;

public class SecondFactorDto
{
    public string Hash { get; set; }
    public string Token { get; set; }
    public long TimeToLive { get; set; }
}

public class UpdateSecondFactorCommand : IRequest<Result<SecondFactorDto>>
{
    public string UserId { get; set; }
    public string AppId { get; set; }
    public bool Enable { get; set; }
}

public class UpdateSecondFactorHandler : IRequestHandler<UpdateSecondFactorCommand, Result<SecondFactorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpService _otpService;

    public UpdateSecondFactorHandler(IUnitOfWork unitOfWork, IOtpService otpService)
    {
        _unitOfWork = unitOfWork;
        _otpService = otpService;
    }

    public async Task<Result<SecondFactorDto>> Handle(
        UpdateSecondFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.Get(
            e => e.AppId == request.AppId && e.Id == request.UserId, cancellationToken);

        if (user is null)
            return Result<SecondFactorDto>.NotFound("User not found");

        user.TwoFactorEnabled = request.Enable;
        user.ConcurrencyStamp = Guid.NewGuid().ToString("N");

        if (!request.Enable)
        {
            var factor = await _unitOfWork.OtpCodes.ReadFactor(request.UserId, request.AppId, cancellationToken);
            if (factor is not null)
                _unitOfWork.OtpCodes.RemoveFactor(factor);

            await _unitOfWork.Save();
            return Result<SecondFactorDto>.Ok(new SecondFactorDto());
        }

        var newFactor = await _otpService.CreateSecondFactor(user, cancellationToken);
        if (!newFactor.Success) return newFactor;

        _unitOfWork.OtpCodes.CreateFactor(new SecondFactor
        {
            UserId = user.Id,
            AppId = user.AppId,
            Token = newFactor.Data.Token,
            Hash = Base32Encoding.ToBytes(newFactor.Data.Hash),
            TimeToLive = newFactor.Data.TimeToLive,
            IsActive = true,
        });

        return newFactor;
    }
}