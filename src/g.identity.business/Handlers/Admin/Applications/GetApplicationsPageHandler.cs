using g.commons.Models;
using g.identity.dataAccess;
using g.identity.dataAccess.Domain;
using MediatR;

namespace g.identity.business.Handlers.Admin.Applications;

public class GetApplicationsPageQuery : IRequest<PagedResult<Application>>
{
    public int Count { get; set; }
    public int Page { get; set; }
}

public class GetApplicationsPageHandler : IRequestHandler<GetApplicationsPageQuery, PagedResult<Application>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetApplicationsPageHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<Application>> Handle(
        GetApplicationsPageQuery request, CancellationToken cancellationToken)
    {
        var (total, data) = await _unitOfWork.Apps.ReadPaged(
            request.Page, request.Count,
            e => true,
            e => e.Created, true,
            cancellationToken);

        return PagedResult<Application>.Ok(data, request.Count, request.Page, total);
    }
}