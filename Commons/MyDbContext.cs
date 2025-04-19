using Microsoft.EntityFrameworkCore;

namespace Commons;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<PessoaEntity> Pessoas { get; set; }
}
