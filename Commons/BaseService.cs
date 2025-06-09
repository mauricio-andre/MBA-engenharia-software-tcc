using System.Diagnostics;
using System.Text;

namespace Commons;

public abstract class BaseService : IBaseService
{
    readonly int[] rateList =
    [
        1,
        10,
        100,
        500,
        1_000,
        5_000,
        10_000,
        50_000,
        100_000,
        250_000,
        500_000,
        750_000,
        1_000_000,
        5_000_000,
        10_000_000,
    ];

    public async Task Run()
    {
        for (int i = 0; i < 10; i++)
        {
            string basePath = PrepareBasePatch();
            await using var relatorioWriter = new StreamWriter(
                Path.Combine(basePath, "relatorio.csv"),
                true,
                Encoding.UTF8);

            foreach (var limit in rateList)
            {
                var stopwatch = Stopwatch.StartNew();

                var csvPath = Path.Combine(basePath, "report", $"pessoas_{limit}.csv");
                await using (var writer = new StreamWriter(csvPath, true, Encoding.UTF8))
                {
                    await writer.WriteLineAsync("Id;Nome;Email;DataNascimento;Cidade");
                    await LoopWriteLines(limit, writer);
                    CloseChannels();
                }

                stopwatch.Stop();

                await relatorioWriter.WriteLineAsync($"{limit};{stopwatch.ElapsedMilliseconds}");

                Console.WriteLine("O bloco {0} processou em {1} milissegundos", limit, stopwatch.ElapsedMilliseconds);
            }
        }
    }

    private static string PrepareBasePatch()
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(basePath);

        var reportPatch = Path.Combine(basePath, "report");
        if (Directory.Exists(reportPatch))
            Directory.Delete(reportPatch, true);

        Directory.CreateDirectory(reportPatch);

        return basePath;
    }

    private async Task LoopWriteLines(int limit, StreamWriter writer)
    {
        var skip = 0;
        var take = limit >= 1000 ? 1000 : limit;

        do
        {
            var result = GetData(skip, take);
            await foreach (var pessoas in result)
            {
                await writer.WriteLineAsync(string.Concat(
                    pessoas.Id,
                    ";",
                    pessoas.Nome,
                    ";",
                    pessoas.Email,
                    ";",
                    pessoas.DataNascimento.ToString("yyyy-mm-dd"),
                    ";",
                    pessoas.Cidade
                ));
            }

            skip += take;
        } while (skip < limit);
    }

    public abstract IAsyncEnumerable<PessoaEntity> GetData(int skip, int take);

    public virtual void CloseChannels()
    {
    }
}
