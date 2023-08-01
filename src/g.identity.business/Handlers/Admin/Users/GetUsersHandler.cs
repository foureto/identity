using System.Linq.Expressions;
using g.commons.Extensions;
using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Users;

public class GetUsersQuery : IRequest<PagedResult<AppUser>>
{
    public int Page { get; set; }
    public int Count { get; set; }
    public string ApplicationId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string NickName { get; set; }
    public string RoleId { get; set; }
}

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PagedResult<AppUser>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<AppUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<AppUser, bool>> expr = e => true;

        expr = string.IsNullOrWhiteSpace(request.Email)
            ? expr
            : expr.And(e => e.Email.ToLower().Contains(request.Email.ToLower()));
        expr = string.IsNullOrWhiteSpace(request.ApplicationId)
            ? expr
            : expr.And(e => e.AppId == request.ApplicationId);
        expr = string.IsNullOrWhiteSpace(request.Phone)
            ? expr
            : expr.And(e => e.PhoneNumber != null && e.PhoneNumber.StartsWith(request.Phone));
        expr = string.IsNullOrWhiteSpace(request.RoleId)
            ? expr
            : expr.And(e => e.UserRoles.Any(r => r.RoleId == request.RoleId));

        var (total, data) = await _unitOfWork.Users.ReadPaged(
            request.Page, request.Count, expr, e => e.Id, true, cancellationToken);
        
        return PagedResult<AppUser>.Ok(data, request.Count, request.Page, total);
    }
}