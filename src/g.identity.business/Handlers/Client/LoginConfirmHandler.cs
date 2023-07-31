using FluentValidation;
using g.commons.Models;
using g.identity.business.Handlers.Client.Models;
using MediatR;

namespace g.identity.business.Handlers.Client;

public class LoginConfirmCommand : IRequest<Result<TokenDto>>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
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
    public LoginConfirmHandler()
    {
        
    }
    
    public Task<Result<TokenDto>> Handle(LoginConfirmCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}