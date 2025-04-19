using Microsoft.EntityFrameworkCore;

namespace Commons;

public class PessoaRepository : IPessoaRepository
{
    private readonly MyDbContext _myDbContext;

    public PessoaRepository(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }

    public IAsyncEnumerable<PessoaEntity> GetPessoas(int skip, int take)
    {
        return _myDbContext.Pessoas
            .OrderBy(pessoa => pessoa.Id)
            .Skip(skip)
            .Take(take)
            .AsAsyncEnumerable();
    }
}
