using System;
using System.Text.Json.Serialization;

namespace Edwards.CodeChallenge.API.ViewModels.User;

public class EdwardsUserViewModel
{
    [JsonConstructor]
    public EdwardsUserViewModel(int id, string firstName, string lastName, string email, string notes, DateTime dateCreated)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Notes = notes;
        DateCreated = dateCreated;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Notes { get; private set; }
    public DateTime DateCreated { get; private set; }



}
