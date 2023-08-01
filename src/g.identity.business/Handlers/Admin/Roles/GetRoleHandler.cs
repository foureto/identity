using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Roles;

public class GetRoleQuery : IRequest<Result<AppRole>>
{
    public string RoleId { get; set; }
}

public class GetRoleHandler : IRequestHandler<GetRoleQuery, Result<AppRole>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<AppRole>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.Read(e => e.Id == request.RoleId, cancellationToken);
        return role is null ? Result<AppRole>.NotFound("Role not found") : Result<AppRole>.Ok(role);
    }
}