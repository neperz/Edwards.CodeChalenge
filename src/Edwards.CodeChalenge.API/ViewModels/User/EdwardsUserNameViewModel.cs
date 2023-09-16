using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Edwards.CodeChalenge.API.ViewModels.User;

public class EdwardsUserNameViewModel
{
    public EdwardsUserNameViewModel()
    {

    }
    public EdwardsUserNameViewModel(string name)
    {
        Name = name;
    }

    [FromRoute(Name = "name")]
    [Required(ErrorMessage = "Name is mandatory")]
    public string Name { get; set; }
}
