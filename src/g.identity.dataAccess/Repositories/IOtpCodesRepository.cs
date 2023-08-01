using g.identity.dataAccess.Domain;

namespace g.identity.dataAccess.Repositories;

public interface IOtpCodesRepository : IBaseRepository<OtpCode>
{
    Task<bool> AddSecondFactor(SecondFactor secondFactor, CancellationToken token = default);
    Task<SecondFactor> ReadFactor(string userId, string appId, CancellationToken token = default);
    void RemoveFactor(SecondFactor factor);
    void CreateFactor(SecondFactor newFactor);
}