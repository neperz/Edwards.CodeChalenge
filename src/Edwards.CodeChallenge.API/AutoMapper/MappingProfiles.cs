using AutoMapper;
using Edwards.CodeChallenge.API.ViewModels.User;
using Edwards.CodeChallenge.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Edwards.CodeChallenge.API.AutoMapper;

[ExcludeFromCodeCoverage]
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        #region EdwardsUser


        CreateMap<EdwardsUser, EdwardsUserViewModel>()
            .ConstructUsing(s => new EdwardsUserViewModel(
                s.Id,
                s.FirstName,
                s.LastName,
                s.Email,
                s.Notes,
                s.DateCreated
            )).ReverseMap();

        #endregion
    }
}
