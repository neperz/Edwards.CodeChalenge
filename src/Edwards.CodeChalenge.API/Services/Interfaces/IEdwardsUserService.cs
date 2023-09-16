using Edwards.CodeChalenge.API.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edwards.CodeChalenge.API.Services.Interfaces;

public interface IEdwardsUserService
{
    Task<IEnumerable<EdwardsUserViewModel>> GetAllAsync();
    Task<EdwardsUserViewModel> GetByIdAsync(EdwardsUserIdViewModel edwardsUserVM);

    Task RemoveAsync(EdwardsUserViewModel edwardsUserVM);
    Task UpdateAsync(EdwardsUserViewModel edwardsUserVM);
    Task<EdwardsUserViewModel> AddAsync(EdwardsUserViewModel edwardsUserVM);
    Task<EdwardsUserViewModel> GetByEmailAsync(EdwardsUserEmailViewModel edwardsUserVM);
    Task<EdwardsUserViewModel> GetByNameAsync(EdwardsUserNameViewModel edwardsUserVM);
}
