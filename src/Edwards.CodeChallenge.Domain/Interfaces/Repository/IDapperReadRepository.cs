using System.Threading.Tasks;

namespace Edwards.CodeChallenge.Domain.Interfaces.Repository;

public interface IDapperReadRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
}
