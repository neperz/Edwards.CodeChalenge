using System;

namespace Edwards.CodeChallenge.Domain.Models;

public class EdwardsUser
{
    protected EdwardsUser() { }

    public EdwardsUser(string id, string firstName, string lastName, string email, string notes) : this(firstName, lastName, email, notes)
    {
        Id = id;
    }

    public EdwardsUser(string firstName, string lastName, string email, string notes)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Notes = notes;
    }

    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Notes { get; private set; }
    public DateTime DateCreated { get; private set; }

}
