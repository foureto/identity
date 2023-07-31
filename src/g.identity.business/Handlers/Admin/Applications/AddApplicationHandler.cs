using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Applications;

public class AddApplicationCommand : IRequest<Result>
{
    public string Id { get; set; }
    public string Name { get; set; }
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
        if (await _unitOfWork.Apps.Any(e => e.Id == request.Id, cancellationToken))
            return Result.Bad("Application already exists");
        
        var entity = _unitOfWork.Apps.AddAndSave(
            new Application { Id = request.Id, Name = request.Name }, cancellationToken);
        
        return Result.Ok($"Application '{request.Name}' created");
    }
}