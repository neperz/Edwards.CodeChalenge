using Edwards.CodeChalenge.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edwards.CodeChalenge.Domain.Interfaces.Repository;

public interface IEdwardsUserRepository : IEntityBaseRepository<EdwardsUser>, IDapperReadRepository<EdwardsUser>
{
    Task<EdwardsUser> GetByNameAsync(string name);
    Task<EdwardsUser> GetByEmailAsync(string email);
    Task<IEnumerable<EdwardsUser>> GetAllAsync();
}
