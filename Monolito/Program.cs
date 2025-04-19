using Commons;
using Microsoft.Extensions.DependencyInjection;
using Monolito;

var services = new ServiceCollection();

await services.AddConfigCommonsAsync();

services.AddScoped<IBaseService, MonolitoService>();

var serviceProvider = services.BuildServiceProvider();
using (var scope = serviceProvider.CreateScope())
{
    var monolitoService = scope.ServiceProvider.GetRequiredService<IBaseService>();

    await monolitoService.Run();
}