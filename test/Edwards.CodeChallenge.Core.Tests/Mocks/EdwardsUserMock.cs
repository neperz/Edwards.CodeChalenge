using Bogus;
using Edwards.CodeChallenge.API.ViewModels.User;
using Edwards.CodeChallenge.Domain.Models;


namespace Edwards.CodeChallenge.Core.Tests.Mocks
{
    public static class EdwardsUserMock
    {
        public static Faker<EdwardsUser> EdwardsUserModelFaker =>
            new Faker<EdwardsUser>()
            .CustomInstantiator(x => new EdwardsUser
            (
                id: x.Random.Guid().ToString(),
                firstName: x.Person.FirstName,
                lastName: x.Person.LastName,
                email: x.Person.Email,
                notes: x.Person.Website
            ));


        public static Faker<EdwardsUserViewModel> EdwardsUserViewModelFaker =>
            new Faker<EdwardsUserViewModel>()
            .CustomInstantiator(x => new EdwardsUserViewModel
            (
                id: x.Random.Guid().ToString(),
                 firstName: x.Person.FirstName,
                lastName: x.Person.LastName,
                email: x.Person.Email,
                notes: x.Person.Website,
                dateCreated: x.Date.Recent()
            ));

        public static Faker<EdwardsUserIdViewModel> EdwardsUserIdViewModelFaker =>
            new Faker<EdwardsUserIdViewModel>()
            .CustomInstantiator(x => new EdwardsUserIdViewModel
            (
                id: x.Random.Guid().ToString()
            ));

        public static Faker<EdwardsUserNameViewModel> EdwardsUserNameViewModelFaker =>
            new Faker<EdwardsUserNameViewModel>()
            .CustomInstantiator(x => new EdwardsUserNameViewModel
            (
                name: x.Person.FirstName
            ));
        public static Faker<EdwardsUserEmailViewModel> EdwardsUserEmailViewModelFaker =>
            new Faker<EdwardsUserEmailViewModel>()
            .CustomInstantiator(x => new EdwardsUserEmailViewModel
            (
                email: x.Person.Email
            ));



    }
}
