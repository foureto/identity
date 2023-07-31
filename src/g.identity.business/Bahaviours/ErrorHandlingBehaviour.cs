using g.commons.Models;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace g.identity.business.Bahaviours;

public interface IIgnoreErrorHandling
{
}

public class ErrorHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ErrorHandlingBehaviour<TRequest, TResponse>> _logger;

    public ErrorHandlingBehaviour(ILogger<ErrorHandlingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            if (request is IIgnoreErrorHandling h)
            {
                _logger.LogDebug("Ignore error handling: {Type}", h.GetType());
                throw;
            }

            _logger.LogError(e, "Could not process request of {Type}", request.GetType().Name);
            var response = Result.Internal("Internal server error").Adapt<TResponse>();

            return response;
        }
    }
}