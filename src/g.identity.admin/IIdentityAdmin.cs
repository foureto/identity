using System.ServiceModel;
using g.commons.Models;
using g.identity.admin.Requests.Applications;
using g.identity.admin.Requests.Roles;
using g.identity.admin.Requests.Users;
using g.identity.admin.Responses.Applications;
using g.identity.admin.Responses.Roles;
using g.identity.admin.Responses.Users;

namespace g.identity.admin;

[ServiceContract]
public interface IIdentityAdmin
{
    // Applications
    [OperationContract]
    ValueTask<Result> AddApplication(AddApplicationRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result> RemoveApplication(RemoveApplicationRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result> UpdateApplication(UpdateApplicationRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result<ApplicationDetailsResponse>> GetApplication(
        GetApplicationRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<PagedResult<ApplicationResponse>> GetApplications(
        GetApplicationsRequest request, CancellationToken token = default);

    // Roles
    [OperationContract]
    ValueTask<Result> AddRole(AddRoleRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result> RemoveRole(RemoveRoleRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result> UpdateRole(UpdateRoleRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result<RoleDetailsResponse>> GetRole(GetRoleRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<PagedResult<RoleResponse>> GetRoles(GetRolesRequest request, CancellationToken token = default);

    // Users
    ValueTask<PagedResult<UserResponse>> GetUsers(GetUsersRequest request, CancellationToken token = default);
    ValueTask<Result<UserDetailsResponse>> GetUser(GetUserRequest request, CancellationToken token = default);
    ValueTask<Result> UpdateUser(UpdateUserRequest request, CancellationToken token = default);
    ValueTask<Result> DeleteUser(DeleteUserRequest request, CancellationToken token = default);
    ValueTask<Result> AddUserRole(AddUserRoleRequest request, CancellationToken token = default);
    ValueTask<Result> RemoveUserRole(RemoveUserRoleRequest request, CancellationToken token = default);
}