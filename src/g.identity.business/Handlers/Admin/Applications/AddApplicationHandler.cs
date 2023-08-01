using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Applications;

public class AddApplicationCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedById { get; set; }
}

public class AddApplicationHandler : IRequestHandler<AddApplicationCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddApplicationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddApplicationCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Apps.AddAndSave(
            new Application
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = request.Name,
                Description = request.Description,
                CreatedById = request.CreatedById,
            }, cancellationToken);

        return Result.Ok($"Application '{request.Name}' created");
    }
}