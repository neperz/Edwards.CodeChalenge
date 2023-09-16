using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Models;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Edwards.CodeChallenge.Domain.Validation.UserValidation;

public class EdwardsUserInsertValidation : AbstractValidator<EdwardsUser>
{
    private readonly IEdwardsUserRepository _edwardsUserRepository;

    public EdwardsUserInsertValidation(IEdwardsUserRepository edwardsUserRepository)
    {
        _edwardsUserRepository = edwardsUserRepository;
        RuleFor(x => x.Id)
        .NotNull()
        .NotEmpty()
        .WithMessage("Id is required.")
        .MustAsync(ValidationId)
         .WithMessage("This Id is already registered in the database");
        ;

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
            .WithMessage("Invalid email address.")
            .MustAsync(ValidationEmail)
            .WithMessage("This email is already registered in the database");
        ;

    }

    private async Task<bool> ValidationEmail(string email, CancellationToken cancellationToken)
    {
        var edwardsUserRepository = await _edwardsUserRepository.GetByEmailAsync(email);

        return edwardsUserRepository == null;
    }
    private async Task<bool> ValidationId(int idUser, CancellationToken cancellationToken)
    {
        var edwardsUserRepository = await _edwardsUserRepository.GetByIdAsync(idUser);

        return edwardsUserRepository == null;
    }
}
