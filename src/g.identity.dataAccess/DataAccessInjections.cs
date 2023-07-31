using g.identity.dataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace g.identity.dataAccess;

public static class DataAccessInjections
{
    private const string DefaultSectionName = "default";

    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = DefaultSectionName)
    {
        return services
            .AddDbContextPool<IdentityAppContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString(sectionName)))
            .AddIdentityCore<AppUser>(options => { options.User.RequireUniqueEmail = false; })
            .Services;
    }
}