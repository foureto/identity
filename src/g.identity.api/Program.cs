using Flour.Logging;
using g.identity.api.Services;
using g.identity.business;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogging();

builder.Services
    .AddBusiness(builder.Configuration)
    .AddCodeFirstGrpcReflection()
    .AddCodeFirstGrpc(opts => { });

var app = builder.Build();

app.MapGrpcService<IdentityAdmin>();
app.MapGrpcService<IdentityClientService>();
app.Run();