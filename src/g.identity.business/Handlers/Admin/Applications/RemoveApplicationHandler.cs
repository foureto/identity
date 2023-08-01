using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Applications;

public class RemoveApplicationCommand : IRequest<Result>
{
    public string ApplicationId { get; set; }
    public string RemovedById { get; set; }
}

public class RemoveApplicationHandler : IRequestHandler<RemoveApplicationCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveApplicationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveApplicationCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Apps.Any(e => e.Id == request.ApplicationId, cancellationToken))
            return Result.Bad("Application does not exists");

        await _unitOfWork.Apps.RemoveAndSave(new Application { Id = request.ApplicationId }, cancellationToken);

        return Result.Ok($"Application '{request.ApplicationId}' removed");
    }
}