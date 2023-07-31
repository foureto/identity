using System.Reflection;
using g.identity.business.Bahaviours;
using g.identity.business.Services;
using g.identity.business.Services.Internals;
using g.identity.dataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace g.identity.business;

public static class BusinessInjections
{
    public static IServiceCollection AddBusiness(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .Configure<AppSettings>(opts => configuration.GetSection("app").Bind(opts))
            .AddScoped<ISessionProvider, SessionProvider>()
            .AddSingleton<IRandomStringGenerator, RandomStringGenerator>()
            .AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
                .AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>))
                .AddOpenBehavior(typeof(RequestValidationBehaviour<,>)))
            .AddDataAccess(configuration);
    }
}