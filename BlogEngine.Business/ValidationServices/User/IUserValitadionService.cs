using BlogEngine.Data.Entities;
using BlogEngine.Data.Identity;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogEngine.Business.ValidationServices.Contracts
{
    public interface IUserValitadionService
    {
        Task<(ValidationResultDto validationResultDto, User UserEntity)> ValidateForCreate(CreateUserCriteriaDto criteria);
        Task<(ValidationResultDto validationResultDto, AppUser identityUser, IdentityRole identityRole)> ValidateUserAndRole(string userName, string role);
        Task<(ValidationResultDto validationResultDto, AppUser identityUser)> ValidateUser(string userName);
        Task<(ValidationResultDto validationResultDto, AppUser identityUser)> ValidateUserEmail(string email);
        Task<(ValidationResultDto validationResultDto, AppUser identityUser)> ValidateForSignIn(string userNameOrEmail, string password);

    }
}
