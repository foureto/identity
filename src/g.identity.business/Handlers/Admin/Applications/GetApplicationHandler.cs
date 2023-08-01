using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Applications;

public class GetApplicationQuery : IRequest<Result<Application>>
{
    public string ApplicationId { get; set; }
}

public class GetApplicationHandler : IRequestHandler<GetApplicationQuery , Result<Application>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetApplicationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Application>> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var app = await _unitOfWork.Apps.Read(e => e.Id == request.ApplicationId, cancellationToken);
        return app is null ? Result<Application>.NotFound("Application not found") : Result<Application>.Ok(app);
    }
}