using Commons;

namespace Monolito;

public class MonolitoService : BaseService
{
    private readonly IPessoaRepository _pessoaRepository;

    public MonolitoService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    public override IAsyncEnumerable<PessoaEntity> GetData(int skip, int take)
    {
        return _pessoaRepository.GetPessoas(skip, take);
    }
}
