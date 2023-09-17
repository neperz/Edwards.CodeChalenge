using Edwards.CodeChallenge.Domain.Models;
using System;
using System.Linq;

namespace Edwards.CodeChallenge.Infra.Context;
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

            _context.Add(new EdwardsUser(id: Guid.NewGuid().ToString().ToUpper(), firstName: "Zier", lastName: "Zuveiku", email: "zier@ed.com", "first note"));
            _context.Add(new EdwardsUser(id: Guid.NewGuid().ToString().ToUpper(), firstName: "Vikehel", lastName: "Pleamakh", email: "vikehel@ed.com", "first note"));
            _context.Add(new EdwardsUser(id: Guid.NewGuid().ToString().ToUpper(), firstName: "Diuor", lastName: "PleaBolosmakh", email: "diuor@ed.com", "first note"));

            _context.SaveChanges();
        }
    }
}
