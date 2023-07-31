using Grpc.Core.Interceptors;

namespace g.identity.commons;

using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using GrpcClientFactory = ProtoBuf.Grpc.Client.GrpcClientFactory;

public static class GrpcInjections
{
    public static IServiceCollection AddGRpcService<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName,
        params Interceptor[] interceptors) where T : class
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        var serviceUrl = configuration.GetValue<string>(sectionName);
        if (interceptors != null)
            foreach (var interceptor in interceptors)
                services.AddTransient(interceptor.GetType());

        return services
            .AddCodeFirstGrpcClient<T>(opts =>
            {
                if (interceptors != null)
                    foreach (var interceptor in interceptors)
                        opts.InterceptorRegistrations.Add(
                            new InterceptorRegistration(
                                InterceptorScope.Client,
                                sp => sp.GetRequiredService(interceptor.GetType()) as Interceptor));

                opts.Address = new Uri(serviceUrl);
                opts.ChannelOptionsActions.Add(channelOpts =>
                {
                    channelOpts.MaxRetryAttempts = 2;
                    channelOpts.MaxReceiveMessageSize = null;
                    channelOpts.DisableResolverServiceConfig = true;
                    channelOpts.HttpHandler = new SocketsHttpHandler
                    {
                        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                        KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                        KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                        EnableMultipleHttp2Connections = true
                    };
                });
            }).Services;
    }

    public static IServiceCollection AddGRpcService<T>(
        this IServiceCollection services,
        Action<IServiceProvider, GrpcClientFactoryOptions> configure) where T : class
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        return services.AddCodeFirstGrpcClient<T>(configure).Services;
    }
}