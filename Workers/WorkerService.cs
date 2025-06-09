using Commons;
using Grpc;
using Grpc.Core;
using Grpc.Net.Client;

namespace Workers;

public class WorkerService : BaseService
{
    private GrpcChannel? _grpcChannel;

    public override async IAsyncEnumerable<PessoaEntity> GetData(int skip, int take)
    {
        var uri = Environment.GetEnvironmentVariable("GRPC_URI") ?? "http://localhost:5153";
        _grpcChannel ??= GrpcChannel.ForAddress(uri);
        var pessoaClient = new Pessoa.PessoaClient(_grpcChannel);

        using var call = pessoaClient.GetPessoaList(new GetPessoaRequest { Skip = skip, Take = take });
        while (await call.ResponseStream.MoveNext())
        {
            var response = call.ResponseStream.Current;
            yield return new PessoaEntity()
            {
                Id = response.Id,
                Nome = response.Nome,
                Email = response.Email,
                DataNascimento = DateTimeOffset.FromUnixTimeSeconds(response.DataNascimentoTimestamp).UtcDateTime,
                Cidade = response.Cidade
            };
        }
    }

    public override void CloseChannels()
    {
        _grpcChannel?.Dispose();
        _grpcChannel = null;
    }
}