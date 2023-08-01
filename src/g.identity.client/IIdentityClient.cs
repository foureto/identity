using System.ServiceModel;
using g.commons.Models;
using g.identity.client.Requests;
using g.identity.client.Responses;

namespace g.identity.client;

[ServiceContract]
public interface IIdentityClient
{
    [OperationContract]
    ValueTask<Result<TokenResponse>> Login(LoginRequest request, CancellationToken token = default);
    
    [OperationContract]
    ValueTask<Result<TokenResponse>> LoginConfirm(LoginConfirmRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result<TokenResponse>> Refresh(RefreshRequest request, CancellationToken token = default);
    
    [OperationContract]
    ValueTask<Result<SecondFactorResponse>> ChangeSecondFactor(
        UpdateSecondFactorRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result> SetUserInfo(SetUserInfoRequest request, CancellationToken token = default);
}