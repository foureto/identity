﻿using g.commons.Models;
using g.identity.dataAccess;
using MediatR;

namespace g.identity.business.Handlers.Admin.Users;

public class UpdateUserCommand : IRequest<Result>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
    public string Phone { get; set; }
    public string NickName { get; set; }
    public string ClientId { get; set; }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.Get(
            e => e.Id == request.UserId && e.AppId == request.ApplicationId, cancellationToken);
        
        if (user is null)
            return Result.NotFound("User not found");

        user.Nickname = request.NickName;
        user.PhoneNumber = request.Phone;
        user.ClientId = request.ClientId;

        await _unitOfWork.Save();
        return Result.Ok("User data updated");
    }
}