using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Edwards.CodeChalenge.API.ViewModels.User;

public class EdwardsUserEmailViewModel
{
    public EdwardsUserEmailViewModel()
    {

    }
    public EdwardsUserEmailViewModel(string email)
    {
        Email = email;
    }

    [FromRoute(Name = "email")]
    [Required(ErrorMessage = "E-mail is mandatory")]
    public string Email { get; set; }
}