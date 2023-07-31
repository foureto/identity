using g.commons.Models;
using g.identity.dataAccess;
using MediatR;

namespace g.identity.business.Handlers.Client;

public class SecondFactorDto
{
    public string Hash { get; set; }
    public string Token { get; set; }
    public long TimeToLive { get; set; }
}

public class UpdateSecondFactorCommand : IRequest<Result<SecondFactorDto>>
{
    
}

public class UpdateSecondFactorHandler : IRequestHandler<UpdateSecondFactorCommand, Result<SecondFactorDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSecondFactorHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<Result<SecondFactorDto>> Handle(UpdateSecondFactorCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}