using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Users;

public class RemoveUserRoleCommand : IRequest<Result>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
    public string RoleId { get; set; }
    public string UpdatedById { get; set; }
}

public class RemoveUserRoleHandler : IRequestHandler<RemoveUserRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveUserRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.UserRoles.Any(
                e => e.RoleId == request.RoleId && e.UserId == request.UserId, cancellationToken))
            return Result.NotFound("Role is not assigned to this user");

        await _unitOfWork.UserRoles.RemoveAndSave(
            new AppUserRole { RoleId = request.RoleId, UserId = request.UserId }, cancellationToken);
        
        return Result.Ok("Role unassigned");
    }
}