using Dapper;
using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Models;
using Edwards.CodeChallenge.Infra.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edwards.CodeChallenge.Infra.Repository
{
    public class EdwardsUserRepository : EntityBaseRepository<EdwardsUser>, IEdwardsUserRepository
    {
        private readonly DapperContext _dapperContext;

        public EdwardsUserRepository(EntityContext context, DapperContext dapperContext)
            : base(context)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<EdwardsUser>> GetAllAsync()
        {
            var query = @"SELECT Id, FirstName, LastName, Email, Notes, DateCreated
                            FROM User c";

            return await _dapperContext.DapperConnection.QueryAsync<EdwardsUser>(query, null, null, null, null);
        }

        public async Task<EdwardsUser> GetByIdAsync(int id)
        {
            var query = @"SELECT Id, FirstName, LastName, Email, Notes, DateCreated
                            FROM User
                          WHERE Id = @Id";

            return (await _dapperContext.DapperConnection.QueryAsync<EdwardsUser>(query, new { Id = id })).FirstOrDefault();
        }



        public async Task<EdwardsUser> GetByNameAsync(string name)
        {
            var query = @"SELECT  Id, FirstName, LastName, Email, Notes, DateCreated
                            FROM User
                          WHERE FirstName = @Name";

            return (await _dapperContext.DapperConnection.QueryAsync<EdwardsUser>(query, new { Name = name })).FirstOrDefault();
        }
        public async Task<EdwardsUser> GetByEmailAsync(string email)
        {
            var query = @"SELECT  Id, FirstName, LastName, Email, Notes, DateCreated
                            FROM User
                          WHERE Email = @email";

            return (await _dapperContext.DapperConnection.QueryAsync<EdwardsUser>(query, new { email = email })).FirstOrDefault();
        }
    }
}
