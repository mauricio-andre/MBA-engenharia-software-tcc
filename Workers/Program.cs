using Commons;
using Microsoft.Extensions.DependencyInjection;
using Workers;

var services = new ServiceCollection();

services.AddScoped<IBaseService, WorkerService>();

var serviceProvider = services.BuildServiceProvider();
using (var scope = serviceProvider.CreateScope())
{
    var monolitoService = scope.ServiceProvider.GetRequiredService<IBaseService>();

    await monolitoService.Run();
}