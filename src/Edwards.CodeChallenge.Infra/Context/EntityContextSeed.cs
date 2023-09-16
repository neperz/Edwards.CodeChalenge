using Edwards.CodeChallenge.Domain.Models;
using System.Linq;

namespace Edwards.CodeChallenge.Infra.Context
{
    public class EntityContextSeed
    {
        private readonly EntityContext _context;

        public EntityContextSeed(EntityContext context)
        {
            this._context = context;
            this.SeedInitial();
        }

        public void SeedInitial()
        {

            if (!_context.EdwardsUsers.Any())
            {
                var users = _context.EdwardsUsers.ToList();

                _context.Add(new EdwardsUser(id: 1, firstName: "Zier", lastName: "Zuveiku", email: "zier#ed.com"));
                _context.Add(new EdwardsUser(id: 2, firstName: "Vikehel", lastName: "Pleamakh", email: "vikehel#ed.com"));
                _context.Add(new EdwardsUser(id: 3, firstName: "Diuor", lastName: "PleaBolosmakh", email: "diuor@ed.com"));

                _context.SaveChanges();
            }
        }
    }
}
