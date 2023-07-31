using FluentValidation;
using g.commons.Models;
using g.identity.business.Handlers.Client.Models;
using MediatR;

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
    public LoginHandler()
    {
        
    }
    
    public Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}