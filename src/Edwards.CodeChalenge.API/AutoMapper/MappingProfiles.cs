using AutoMapper;
using Edwards.CodeChalenge.API.ViewModels.User;
using Edwards.CodeChalenge.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Edwards.CodeChalenge.API.AutoMapper;

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
