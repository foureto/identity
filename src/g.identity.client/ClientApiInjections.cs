using g.identity.commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace g.identity.client;

public static class ClientApiInjections
{
    private const string DefaultSection = "api:identity";

    public static IServiceCollection AddClientIdentity(
        this IServiceCollection services, IConfiguration configuration, string sectionName = DefaultSection)
        => services
            .AddGRpcService<IIdentityClient>(configuration, sectionName);
}