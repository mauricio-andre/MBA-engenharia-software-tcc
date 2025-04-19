using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commons;

public static class IServiceCollectionExtension
{
    public static async Task AddConfigCommonsAsync(this IServiceCollection services)
    {
        services.AddScoped<IPessoaRepository, PessoaRepository>();

        services.AddDbContext<MyDbContext>(options =>
        {
            var connection = Environment.GetEnvironmentVariable("DB_PATH") ?? "../Commons/Data/database.db";
            options.UseSqlite($"Data Source={connection}");
        });

        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            await db.Database.EnsureCreatedAsync();

            if (await db.Pessoas.AnyAsync())
            {
                Console.WriteLine("Base já populada.");
                return;
            }

            await SeedHelper.SeedDatabaseAsync(db);
        }
    }
}