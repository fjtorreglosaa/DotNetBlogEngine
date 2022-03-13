using BlogEngine.App.Dto.Auth;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Business.AuthServices.Contracts
{
    public interface IUserAuthService
    {
        Task<(ValidationResultDto ValidationResultDto, UserDto User)> Create(CreateUserCriteriaDto criteria);
        List<UserDto> Get();
        Task<(ValidationResultDto ValidationResultDto, UserDto User)> GetByEmail(string email);    
        Task<(ValidationResultDto ValidationResultDto, UserDto User)> GetByUserName(string userName);               
        Task<(ValidationResultDto ValidationResultDto, UserDto User)> SignIn(LogInCriteriaDto criteria);
        void SignOut();
        Task<(ValidationResultDto ValidationResultDto, UserDto User)> AddRole(string userName, string role);
        Task<(ValidationResultDto ValidationResultDto, UserDto User)> RemoveRole(string userName, string role);

        Task<UserDto> GetcurrentUser(System.Security.Claims.ClaimsPrincipal user);
        
    }
}
