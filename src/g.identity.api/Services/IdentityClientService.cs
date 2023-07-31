using g.commons.Models;
using g.identity.business.Handlers.Client;
using g.identity.business.Handlers.Client.Models;
using g.identity.business.Services;
using g.identity.client;
using g.identity.client.Requests;
using g.identity.client.Responses;
using MediatR;

namespace g.identity.api.Services;

internal class IdentityClientService : MediatedService, IIdentityClient
{
    public IdentityClientService(IMediator mediator, ILogger<IdentityClientService> logger) : base(mediator, logger)
    {
    }

    public ValueTask<Result<TokenResponse>> Login(LoginRequest request, CancellationToken token = default)
        => Mediate<LoginRequest, LoginCommand, TokenDto, TokenResponse>(request, token);

    public ValueTask<Result<TokenResponse>> LoginConfirm(LoginConfirmRequest request, CancellationToken token = default)
        => Mediate<LoginConfirmRequest, LoginConfirmCommand, TokenDto, TokenResponse>(request, token);

    public ValueTask<Result<TokenResponse>> Refresh(RefreshRequest request, CancellationToken token = default)
        => Mediate<RefreshRequest, RefreshCommand, TokenDto, TokenResponse>(request, token);

    public ValueTask<Result<SecondFactorResponse>> ChangeSecondFactor(
        UpdateSecondFactorRequest request, CancellationToken token = default)
        => Mediate<
            UpdateSecondFactorRequest,
            UpdateSecondFactorCommand,
            SecondFactorDto, 
            SecondFactorResponse>(request, token);
}