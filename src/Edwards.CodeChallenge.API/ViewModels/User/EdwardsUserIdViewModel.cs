using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Edwards.CodeChallenge.API.ViewModels.User;

public class EdwardsUserIdViewModel
{
    public EdwardsUserIdViewModel()
    {

    }
    public EdwardsUserIdViewModel(int id)
    {
        Id = id;
    }

    [FromRoute(Name = "id")]
    [Required(ErrorMessage = "Id is mandatory")]
    public int Id { get; set; }
}
