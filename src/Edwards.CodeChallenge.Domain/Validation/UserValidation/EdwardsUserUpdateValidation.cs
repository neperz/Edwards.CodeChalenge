using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Models;
using FluentValidation;

namespace Edwards.CodeChallenge.Domain.Validation.UserValidation;

public class EdwardsUserUpdateValidation : AbstractValidator<EdwardsUser>
{
     

    public EdwardsUserUpdateValidation()
    { 
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id cannot be null");

        RuleFor(person => person.FirstName)
                   .NotEmpty()
                   .Matches(@"^[a-zA-Z\s]+$")
                   .WithMessage("First name must not contain numbers.");

        RuleFor(person => person.LastName)
            .NotEmpty()
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Last name must not contain numbers.");

        RuleFor(x => x.Email)
                  .NotEmpty()
                  .WithMessage("Email is required.")
                  .EmailAddress()
                  .WithMessage("Invalid email address.");

    }

}
