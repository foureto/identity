using FluentValidation;
using g.commons.Models;
using g.identity.business.Handlers.Client.Models;
using g.identity.business.Services;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace g.identity.business.Handlers.Client;

public class LoginCommand : IRequest<Result<TokenDto>>
{
    public string ApplicationId { get; set; }
    public string Email { get; set; }
}

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(e => e.ApplicationId).NotEmpty().WithMessage("Application is not set");
        RuleFor(e => e.Email).NotEmpty().WithMessage("Email is not set");
    }
}

public class LoginHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRandomStringGenerator _generator;
    private readonly IOtpService _oTps;
    private readonly ISessionProvider _sessionProvider;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        IUnitOfWork unitOfWork,
        IRandomStringGenerator generator,
        IOtpService oTps,
        ISessionProvider sessionProvider,
        UserManager<AppUser> userManager,
        ILogger<LoginHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _generator = generator;
        _oTps = oTps;
        _sessionProvider = sessionProvider;
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Apps.Any(e => e.Id == request.ApplicationId, cancellationToken))
            return Result<TokenDto>.Bad("Unknown application request");
        
        var exists = await _unitOfWork.Users.Read(
            e => e.AppId == request.ApplicationId && e.Email.ToLower() == request.Email.ToLower(), cancellationToken) ??
                     await CreateNewUser(request, cancellationToken);

        if (exists is null)
            return Result<TokenDto>.Failed("Could not login");

        var token = await _sessionProvider.NewToken(exists.Id, true);
        if (!token.Success)
            return Result<TokenDto>.Failed(token);

        var code = await _oTps.NotifyWithCode(exists, token.Data.jti, cancellationToken);
        if (!code.Success)
            return Result<TokenDto>.Failed(code);
        
        return Result<TokenDto>.Ok(new TokenDto
        {
            Code = code.Data,
            AccessToken = token.Data.jwt,
            RefreshToken = token.Data.refresh,
        });
    }

    private async Task<AppUser> CreateNewUser(LoginCommand request, CancellationToken token)
    {
        var nick = _generator.Generate(12);
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString("N"),
            AppId = request.ApplicationId,
            Email = request.Email.ToLower(),
            NormalizedEmail = request.Email.ToUpper(),
            Nickname = nick,
            ClientId = $"{nick}_{request.ApplicationId}",
        };

        var ticket = await _userManager.CreateAsync(user);
        if (ticket.Succeeded)
            return user;
        
        _logger.LogError("Could not create new user: {@Errors}", ticket.Errors);
        return null;
    }
}