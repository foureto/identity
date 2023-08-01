﻿using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Users;

public class AddUserRoleCommand : IRequest<Result>
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
    public string RoleId { get; set; }
    public string UpdatedById { get; set; }
}

public class AddUserRoleHandler : IRequestHandler<AddUserRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddUserRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UserRoles.Any(
                e => e.RoleId == request.RoleId && e.UserId == request.UserId, cancellationToken))
            return Result.Bad("Role already assigned");

        await _unitOfWork.UserRoles.AddAndSave(
            new AppUserRole { RoleId = request.RoleId, UserId = request.UserId }, cancellationToken);
        
        return Result.Ok("Role assigned");
    }
}