using System.Runtime.CompilerServices;
using g.commons.Models;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace g.identity.business.Services;

public class MediatedService
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public MediatedService(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async ValueTask<Result> Mediate<TReq, TBReq>(
        TReq request,
        CancellationToken token = default,
        [CallerMemberName] string caller = null)
        where TBReq : IRequest<Result>
    {
        var businessRequest = request.Adapt<TBReq>();
        try
        {
            var businessResponse = await _mediator.Send(businessRequest, token);
            if (!businessResponse.Success)
                _logger.LogWarning("Error: Request {Name} ended with {Message}", caller, businessResponse.Message);

            return businessResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not perform call {Name}. Something went wrong", caller);
            throw;
        }
    }

    public async ValueTask<Result<TRes>> Mediate<TReq, TBReq, TBRes, TRes>(
        TReq request,
        CancellationToken token = default,
        [CallerMemberName] string caller = null)
        where TBReq : IRequest<Result<TBRes>>
    {
        var businessRequest = request.Adapt<TBReq>();
        try
        {
            var businessResponse = await _mediator.Send(businessRequest, token);
            if (businessResponse.Success)
                return Result<TRes>.Ok(businessResponse.Data.Adapt<TRes>(), businessResponse.Message);

            _logger.LogWarning("Error: Request {Name} ended with {Message}", caller, businessResponse.Message);
            return Result<TRes>.Failed(businessResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not perform call {Name}. Something went wrong", caller);
            throw;
        }
    }

    public async ValueTask<ResultList<TRes>> MediateList<TReq, TBReq, TBRes, TRes>(
        TReq request,
        CancellationToken token = default,
        [CallerMemberName] string caller = null)
        where TBReq : IRequest<ResultList<TBRes>>
    {
        var businessRequest = request.Adapt<TBReq>();
        try
        {
            var businessResponse = await _mediator.Send(businessRequest, token);
            if (businessResponse.Success)
                return ResultList<TRes>.Ok(
                    businessResponse.Data?.Select(e => e.Adapt<TRes>()) ?? new List<TRes>(),
                    businessResponse.Message);

            _logger.LogWarning("Error: Request {Name} ended with {Message}", caller, businessResponse.Message);
            return ResultList<TRes>.Failed(businessResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not perform call {Name}. Something went wrong", caller);
            throw;
        }
    }

    public async ValueTask<PagedResult<TRes>> MediatePagedList<TReq, TBReq, TBRes, TRes>(
        TReq request,
        CancellationToken token = default,
        [CallerMemberName] string caller = null)
        where TBReq : IRequest<PagedResult<TBRes>>
    {
        var businessRequest = request.Adapt<TBReq>();
        try
        {
            var businessResponse = await _mediator.Send(businessRequest, token);
            if (businessResponse.Success)
                return PagedResult<TRes>.Ok(
                    businessResponse.Data?.Select(e => e.Adapt<TRes>()) ?? new List<TRes>(),
                    businessResponse.Count,
                    businessResponse.Page,
                    businessResponse.Total);

            _logger.LogWarning("Error: Request {Name} ended with {Message}", caller, businessResponse.Message);
            return PagedResult<TRes>.Failed(businessResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not perform call {Name}. Something went wrong", caller);
            throw;
        }
    }
}