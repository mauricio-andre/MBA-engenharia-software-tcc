namespace Commons;

public interface IPessoaRepository
{
    public IAsyncEnumerable<PessoaEntity> GetPessoas(int skip, int take);
}