using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Edwards.CodeChallenge.API.ViewModels.User;

public class EdwardsUserIdViewModel
{
    public EdwardsUserIdViewModel()
    {

    }
    public EdwardsUserIdViewModel(string id)
    {
        Id = id;
    }

    [FromRoute(Name = "id")]
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }
}
