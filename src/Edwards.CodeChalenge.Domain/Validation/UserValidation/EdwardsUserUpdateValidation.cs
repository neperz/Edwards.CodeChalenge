using Edwards.CodeChalenge.Domain.Interfaces.Repository;
using Edwards.CodeChalenge.Domain.Models;
using FluentValidation;

namespace Edwards.CodeChalenge.Domain.Validation.UserValidation;

public class EdwardsUserUpdateValidation : AbstractValidator<EdwardsUser>
{
    private readonly IEdwardsUserRepository _edwardsUserRepository;

    public EdwardsUserUpdateValidation(IEdwardsUserRepository edwardsUserRepository)
    {
        _edwardsUserRepository = edwardsUserRepository;

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
