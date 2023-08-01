using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Roles;

public class RemoveRoleCommand : IRequest<Result>
{
    public string RoleId { get; set; }
    public string RemovedById { get; set; }
}

public class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Roles.RemoveAndSave(new AppRole { Id = request.RoleId }, cancellationToken);
        return Result.Ok("Role removed");
    }
}