using g.commons.Models;
using g.identity.dataAccess;
using MediatR;

namespace g.identity.business.Handlers.Client;

public class SetUserInfoCommand : IRequest<Result>
{
    public string ApplicationId { get; set; }
    public string UserId { get; set; }
    public string Phone { get; set; }
    public string NickName { get; set; }
}

public class SetUserInfoHandler : IRequestHandler<SetUserInfoCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetUserInfoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(SetUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.Get(
            e => e.Id == request.UserId && e.AppId == request.ApplicationId, cancellationToken);
        
        if (user is null)
            return Result.NotFound("User not found");

        user.Nickname = request.NickName;
        user.PhoneNumber = request.Phone;

        await _unitOfWork.Save();
        return Result.Ok("User data updated");
    }
}