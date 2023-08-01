using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Users;

public class GetUserQuery : IRequest<Result<AppUser>>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
}

public class GetUserHandler : IRequestHandler<GetUserQuery, Result<AppUser>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AppUser>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.Read(
            e => e.Id == request.ApplicationId && e.AppId == request.ApplicationId,
            cancellationToken, e => e.UserRoles);

        return user is null ? Result<AppUser>.NotFound("User not found") : Result<AppUser>.Ok(user);
    }
}