using System.Net;
using Commons;
using Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

await builder.Services.AddConfigCommonsAsync();

var app = builder.Build();

app.MapGrpcService<GreeterService>();

await app.RunAsync();