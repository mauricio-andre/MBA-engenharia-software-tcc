using Grpc.Core;
using Commons;

namespace Grpc.Services;

public class GreeterService : Pessoa.PessoaBase
{
    private readonly IPessoaRepository _pessoaRepository;
    public GreeterService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    public override async Task GetPessoaList(
        GetPessoaRequest request,
        IServerStreamWriter<PessoaReply> responseStream,
        ServerCallContext context)
    {
        await foreach (var pessoa in _pessoaRepository.GetPessoas(request.Skip, request.Take))
        {
            await responseStream.WriteAsync(new PessoaReply
            {
                Nome = pessoa.Nome,
                Email = pessoa.Email,
                DataNascimentoTimestamp = new DateTimeOffset(pessoa.DataNascimento).ToUnixTimeSeconds(),
                Cidade = pessoa.Cidade
            });
        }
    }
}
