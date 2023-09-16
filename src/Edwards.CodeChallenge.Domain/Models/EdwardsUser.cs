using System;

namespace Edwards.CodeChallenge.Domain.Models;

public class EdwardsUser
{
    protected EdwardsUser() { }

    public EdwardsUser(int id, string firstName, string lastName, string email) : this(firstName, lastName, email)
    {
        Id = id;
    }

    public EdwardsUser(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Notes { get; private set; }
    public DateTime DateCreated { get; private set; }

}
