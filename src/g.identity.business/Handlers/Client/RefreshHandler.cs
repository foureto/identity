using g.commons.Models;
using g.identity.business.Handlers.Client.Models;
using MediatR;

namespace g.identity.business.Handlers.Client;

public class RefreshCommand : IRequest<Result<TokenDto>>
{
    public string RefreshToken { get; set; }
}

public class RefreshHandler : IRequestHandler<RefreshCommand, Result<TokenDto>>
{
    public RefreshHandler()
    {
        
    }
    
    public Task<Result<TokenDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}