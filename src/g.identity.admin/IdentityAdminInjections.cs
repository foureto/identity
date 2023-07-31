using g.identity.commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace g.identity.admin;

public static class IdentityAdminInjections
{
    private const string DefaultSection = "apis:identity";

    public static IServiceCollection AddIdentityAdmin(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = DefaultSection)
    {
        return services.AddGRpcService<IIdentityAdmin>(configuration, sectionName);
    }
}