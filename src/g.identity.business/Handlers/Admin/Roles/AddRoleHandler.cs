using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Roles;

public class AddRoleCommand : IRequest<Result>
{
    public string ApplicationId { get; set; }
    public string Name { get; set; }
    public string CreatedById { get; set; }
}

public class AddRoleHandler : IRequestHandler<AddRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Roles.Any(e => e.Name == request.Name, cancellationToken))
            return Result.Bad($"Role {request.Name} already exists");

        await _unitOfWork.Roles.AddAndSave(new AppRole
        {
            CreatedById = request.CreatedById,
            Name = request.Name,
            AppId = request.ApplicationId,
            NormalizedName = request.Name.ToUpper()
        }, cancellationToken);

        return Result.Ok($"New role {request.Name} created");
    }
}