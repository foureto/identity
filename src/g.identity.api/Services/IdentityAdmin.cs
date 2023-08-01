using g.commons.Models;
using g.identity.admin;
using g.identity.admin.Requests.Applications;
using g.identity.admin.Requests.Roles;
using g.identity.admin.Requests.Users;
using g.identity.admin.Responses.Applications;
using g.identity.admin.Responses.Roles;
using g.identity.admin.Responses.Users;
using g.identity.business.Handlers.Admin.Applications;
using g.identity.business.Handlers.Admin.Roles;
using g.identity.business.Handlers.Admin.Users;
using g.identity.business.Services;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.api.Services;

internal class IdentityAdmin : MediatedService, IIdentityAdmin
{
    public IdentityAdmin(IMediator mediator, ILogger<IdentityAdmin> logger) : base(mediator, logger)
    {
    }

    public ValueTask<Result> AddApplication(AddApplicationRequest request, CancellationToken token = default)
        => Mediate<AddApplicationRequest, AddApplicationCommand>(request, token);

    public ValueTask<Result> RemoveApplication(RemoveApplicationRequest request, CancellationToken token = default)
        => Mediate<RemoveApplicationRequest, RemoveApplicationCommand>(request, token);

    public ValueTask<Result> UpdateApplication(UpdateApplicationRequest request, CancellationToken token = default)
        => Mediate<UpdateApplicationRequest, UpdateApplicationCommand>(request, token);

    public ValueTask<Result<ApplicationDetailsResponse>> GetApplication(
        GetApplicationRequest request, CancellationToken token = default)
        => Mediate<GetApplicationRequest, GetApplicationQuery, Application, ApplicationDetailsResponse>(request, token);

    public ValueTask<PagedResult<ApplicationResponse>> GetApplications(
        GetApplicationsRequest request, CancellationToken token = default)
        => MediatePagedList<
            GetApplicationsRequest, GetApplicationsPageQuery, Application, ApplicationResponse>(request, token);

    public ValueTask<Result> AddRole(AddRoleRequest request, CancellationToken token = default)
        => Mediate<AddRoleRequest, AddRoleCommand>(request, token);

    public ValueTask<Result> RemoveRole(RemoveRoleRequest request, CancellationToken token = default)
        => Mediate<RemoveRoleRequest, RemoveRoleCommand>(request, token);

    public ValueTask<Result> UpdateRole(UpdateRoleRequest request, CancellationToken token = default)
        => Mediate<UpdateRoleRequest, UpdateRoleCommand>(request, token);

    public ValueTask<Result<RoleDetailsResponse>> GetRole(GetRoleRequest request, CancellationToken token = default)
        => Mediate<GetRoleRequest, GetRoleQuery, AppRole, RoleDetailsResponse>(request, token);

    public ValueTask<PagedResult<RoleResponse>> GetRoles(GetRolesRequest request, CancellationToken token = default)
        => MediatePagedList<GetRolesRequest, GetRolesQuery, AppRole, RoleResponse>(request, token);

    public ValueTask<PagedResult<UserResponse>> GetUsers(GetUsersRequest request, CancellationToken token = default)
        => MediatePagedList<GetUsersRequest, GetUsersQuery, AppUser, UserResponse>(request, token);

    public ValueTask<Result<UserDetailsResponse>> GetUser(GetUserRequest request, CancellationToken token = default)
        => Mediate<GetUserRequest, GetUserQuery, AppUser, UserDetailsResponse>(request, token);

    public ValueTask<Result> UpdateUser(UpdateUserRequest request, CancellationToken token = default)
        => Mediate<UpdateUserRequest, UpdateUserCommand>(request, token);

    public ValueTask<Result> DeleteUser(DeleteUserRequest request, CancellationToken token = default)
        => Mediate<DeleteUserRequest, DeleteUserCommand>(request, token);

    public ValueTask<Result> AddUserRole(AddUserRoleRequest request, CancellationToken token = default)
        => Mediate<AddUserRoleRequest, AddUserRoleCommand>(request, token);

    public ValueTask<Result> RemoveUserRole(RemoveUserRoleRequest request, CancellationToken token = default)
        => Mediate<RemoveUserRoleRequest, RemoveUserRoleCommand>(request, token);
}