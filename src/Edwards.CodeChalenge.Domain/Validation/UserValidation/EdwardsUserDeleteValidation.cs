using Edwards.CodeChalenge.Domain.Models;
using FluentValidation;

namespace Edwards.CodeChalenge.Domain.Validation.UserValidation;

public class EdwardsUserDeleteValidation : AbstractValidator<EdwardsUser>
{
    public EdwardsUserDeleteValidation()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id cannot be null");
    }
}
