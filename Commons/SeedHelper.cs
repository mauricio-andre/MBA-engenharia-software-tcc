using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Commons;

public static class SeedHelper
{
    public static async Task SeedDatabaseAsync(MyDbContext db)
    {
        if (await db.Pessoas.AnyAsync()) return;

        var faker = new Faker<PessoaEntity>()
            .RuleFor(p => p.Nome, f => f.Name.FullName())
            .RuleFor(p => p.Email, (f, p) => f.Internet.Email(p.Nome.ToLower().Replace(" ", ".")))
            .RuleFor(p => p.DataNascimento, f => f.Date.Past(40, DateTime.Today.AddYears(-18)))
            .RuleFor(p => p.Cidade, f => f.Address.City());

        const int total = 1_000_000;
        const int batchSize = 10_000;

        for (int i = 0; i < total; i += batchSize)
        {
            var pessoas = faker.Generate(batchSize);
            await db.Pessoas.AddRangeAsync(pessoas);
            await db.SaveChangesAsync();
            Console.WriteLine($"Inseridos {i + batchSize} registros...");
        }
    }
}