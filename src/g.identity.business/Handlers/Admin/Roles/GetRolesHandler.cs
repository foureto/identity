using System.Linq.Expressions;
using g.commons.Extensions;
using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Roles;

public class GetRolesQuery : IRequest<PagedResult<AppRole>>
{
    public int Page { get; set; }
    public int Count { get; set; }
    public string ApplicationId { get; set; }
}

public class GetRolesHandler : IRequestHandler<GetRolesQuery, PagedResult<AppRole>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRolesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<AppRole>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<AppRole, bool>> expr = e => true;
        expr = string.IsNullOrWhiteSpace(request.ApplicationId)
            ? expr
            : expr.And(e => e.AppId == request.ApplicationId);

        var (total, data) = await _unitOfWork.Roles.ReadPaged(
            request.Page, request.Count, expr, e => e.Name, false, cancellationToken);

        return PagedResult<AppRole>.Ok(data, request.Count, request.Page, total);
    }
}