using g.commons.Models;
using g.identity.dataAccess;
using MediatR;

namespace g.identity.business.Handlers.Admin.Roles;

public class UpdateRoleCommand : IRequest<Result>
{
    public string RoleId { get; set; }
    public string ApplicationId { get; set; }
    public string Name { get; set; }
    public string UpdatedById { get; set; }
}

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.Get(e => e.Id == request.RoleId, cancellationToken);
        if (role is null)
            return Result.NotFound("Role not found");

        role.Name = request.Name;
        role.AppId = request.ApplicationId;
        role.UpdatedById = request.UpdatedById;

        await _unitOfWork.Save();
        return Result.Ok("Role updated");
    }
}