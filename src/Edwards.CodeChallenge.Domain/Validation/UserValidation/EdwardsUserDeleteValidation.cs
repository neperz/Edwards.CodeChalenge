using Edwards.CodeChallenge.Domain.Models;
using FluentValidation;

namespace Edwards.CodeChallenge.Domain.Validation.UserValidation;

public class EdwardsUserDeleteValidation : AbstractValidator<EdwardsUser>
{
    public EdwardsUserDeleteValidation()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id cannot be null");
    }
}
