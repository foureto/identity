using g.commons.Models;
using g.identity.dataAccess;
using MediatR;

namespace g.identity.business.Handlers.Admin.Applications;

public class UpdateApplicationCommand : IRequest<Result>
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class UpdateApplicationHandler : IRequestHandler<UpdateApplicationCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateApplicationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var app = await _unitOfWork.Apps.Read(e => e.Id == request.Id, cancellationToken);
        if (app is null)
            return Result.NotFound("Application not found");

        if (app.Name == request.Name)
            return Result.Ok();
        
        app.Name = request.Name;
        await _unitOfWork.Apps.UpdateAndSave(app, cancellationToken);
        
        return Result.Ok("Application updated");
    }
}