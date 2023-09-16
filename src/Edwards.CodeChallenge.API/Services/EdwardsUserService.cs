using AutoMapper;
using Edwards.CodeChallenge.API.Services.Interfaces;
using Edwards.CodeChallenge.API.ViewModels.User;
using Edwards.CodeChallenge.Domain.Interfaces.Notifications;
using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Interfaces.UoW;
using Edwards.CodeChallenge.Domain.Models;
using Edwards.CodeChallenge.Domain.Validation.UserValidation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edwards.CodeChallenge.API.Services;

public class EdwardsUserService : IEdwardsUserService
{
    private readonly IEdwardsUserRepository _edwardsUserRepository;
    private readonly IDomainNotification _domainNotification;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ConcurrentDictionary<string, EdwardsUserViewModel> _cache;
    public EdwardsUserService(IEdwardsUserRepository edwardsUserRepository, ConcurrentDictionary<string, EdwardsUserViewModel> cache, IDomainNotification domainNotification, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _edwardsUserRepository = edwardsUserRepository;
        _domainNotification = domainNotification;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IEnumerable<EdwardsUserViewModel>> GetAllAsync()
    {
        var edwardsUsers = _mapper.Map<IEnumerable<EdwardsUserViewModel>>(await _edwardsUserRepository.GetAllAsync());

        return edwardsUsers;
    }

    public async Task<EdwardsUserViewModel> GetByIdAsync(EdwardsUserIdViewModel edwardsUserVM)
    {
        if (_cache.TryGetValue(edwardsUserVM.Id, out EdwardsUserViewModel user))
        {
            return user;
        }
        else
        {
            user = _mapper.Map<EdwardsUserViewModel>(await _edwardsUserRepository.GetByIdAsync(edwardsUserVM.Id));
            _cache.TryAdd(edwardsUserVM.Id, user);
        }
        return user;
    }
    public async Task<EdwardsUserViewModel> GetByEmailAsync(EdwardsUserEmailViewModel edwardsUserVM)
    {
        return _mapper.Map<EdwardsUserViewModel>(await _edwardsUserRepository.GetByEmailAsync(edwardsUserVM.Email));
    }
    public async Task<EdwardsUserViewModel> GetByNameAsync(EdwardsUserNameViewModel edwardsUserVM)
    {
        return _mapper.Map<EdwardsUserViewModel>(await _edwardsUserRepository.GetByNameAsync(edwardsUserVM.Name));
    }
    public async Task<EdwardsUserViewModel> AddAsync(EdwardsUserViewModel edwardsUserVM)
    {
        EdwardsUserViewModel viewModel = null;
        //set de UUD fro user identification
        edwardsUserVM.Id = Guid.NewGuid().ToString().ToUpper();
        var model = _mapper.Map<EdwardsUser>(edwardsUserVM);
        
        var validation = await new EdwardsUserInsertValidation(_edwardsUserRepository).ValidateAsync(model);

        if (!validation.IsValid)
        {
            _domainNotification.AddNotifications(validation);
            return viewModel;
        }

        _edwardsUserRepository.Add(model);
        //add to cache memory
        _cache.TryAdd(model.Id, edwardsUserVM);
        _unitOfWork.Commit();

        viewModel = _mapper.Map<EdwardsUserViewModel>(model);

        return viewModel;
    }

    public async Task UpdateAsync(EdwardsUserViewModel edwardsUserVM)
    {
        var model = _mapper.Map<EdwardsUser>(edwardsUserVM);

        var validation = await new EdwardsUserUpdateValidation(_edwardsUserRepository).ValidateAsync(model);

        if (!validation.IsValid)
        {
            _domainNotification.AddNotifications(validation);
            return;
        }

        _edwardsUserRepository.Update(model);
        if (_cache.TryGetValue(edwardsUserVM.Id, out var value))
        {
            _cache.TryUpdate(edwardsUserVM.Id, edwardsUserVM, value);
        }
        _unitOfWork.Commit();
    }

    public async Task RemoveAsync(EdwardsUserViewModel edwardsUserVM)
    {
        var model = _mapper.Map<EdwardsUser>(edwardsUserVM);

        var validation = await new EdwardsUserDeleteValidation().ValidateAsync(model);

        if (!validation.IsValid)
        {
            _domainNotification.AddNotifications(validation);
            return;
        }

        _edwardsUserRepository.Remove(model);

        _cache.TryRemove(model.Id, out var _);

        _unitOfWork.Commit();
    }
}
