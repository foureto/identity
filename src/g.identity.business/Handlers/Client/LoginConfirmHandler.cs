using FluentValidation;
using g.commons.Models;
using g.identity.business.Handlers.Client.Models;
using g.identity.business.Services;
using g.identity.dataAccess;
using MediatR;

namespace g.identity.business.Handlers.Client;

public class LoginConfirmCommand : IRequest<Result<TokenDto>>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
    public string Jti { get; set; }
    public string Code { get; set; }
}

public class LoginConfirmValidator : AbstractValidator<LoginConfirmCommand>
{
    public LoginConfirmValidator()
    {
        RuleFor(e => e.ApplicationId).NotEmpty().WithMessage("Application is not set");
        RuleFor(e => e.UserId).NotEmpty().WithMessage("User is not defined");
        RuleFor(e => e.Code).NotEmpty().WithMessage("Email is not set");
    }
}

public class LoginConfirmHandler : IRequestHandler<LoginConfirmCommand, Result<TokenDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionProvider _sessionProvider;
    private readonly IOtpService _oTps;

    public LoginConfirmHandler(IUnitOfWork unitOfWork, ISessionProvider sessionProvider, IOtpService oTps)
    {
        _unitOfWork = unitOfWork;
        _sessionProvider = sessionProvider;
        _oTps = oTps;
    }

    public async Task<Result<TokenDto>> Handle(LoginConfirmCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .Read(e => e.Id == request.UserId && e.AppId == request.ApplicationId, cancellationToken);

        if (user is null)
            return Result<TokenDto>.NotFound("User not found");

        var valid = await _oTps.ValidateCode(
            user, request.Jti, request.Code, user.TwoFactorEnabled, cancellationToken);
        if (!valid.Success)
            return Result<TokenDto>.Failed(valid);

        if (user.TwoFactorEnabled)
        {
            return Result<TokenDto>.Ok(new TokenDto { RequiresSecondFactor = true });
        }

        var token = await _sessionProvider.NewToken(user.Id, user.TwoFactorEnabled);
        return token.Success
            ? Result<TokenDto>.Ok(new TokenDto
            {
                AccessToken = token.Data.jwt,
                RefreshToken = token.Data.refresh,
                RequiresSecondFactor = user.TwoFactorEnabled
            })
            : Result<TokenDto>.Failed(token);
    }
}