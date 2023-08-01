using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Users;

public class DeleteUserCommand : IRequest<Result>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
    public string RemovedById { get; set; }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.Any(
                e => e.Id == request.UserId && e.AppId == request.ApplicationId, cancellationToken))
            return Result.NotFound("User not found");

        await _unitOfWork.Users.RemoveAndSave(new AppUser { Id = request.UserId, AppId = request.ApplicationId}, cancellationToken);

        return Result.Ok("User removed");
    }
}