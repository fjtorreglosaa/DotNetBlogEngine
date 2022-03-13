using BlogEngine.App.Dto.Auth;
using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Business.Extensions.Auth;
using BlogEngine.Business.ValidationServices.Contracts;
using BlogEngine.Data.Identity;
using BlogEngine.Data.Repositories.Interfaces;
using BlogEngine.Data.UnitOfWork;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Business.AuthServices
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserValitadionService _userValidationService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;

        public UserAuthService(
            IUserValitadionService userValidationService, 
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IRoleRepository roleRepository)
        {
            _userValidationService = userValidationService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        } 

        public async Task<(ValidationResultDto ValidationResultDto, UserDto User)> Create(CreateUserCriteriaDto criteria)
        {
            var userResult = await _userValidationService.ValidateForCreate(criteria);

            if (userResult.validationResultDto.Conditions.Any())
            {
                return (userResult.validationResultDto, null);
            }

            var identityUser = await _userRepository.Create(userResult.UserEntity);

            if (!identityUser.Succeeded)
            {
                var validationResult = new ValidationResultDto
                {
                    Conditions = new List<ValidationConditionDto>
                    {
                        new ValidationConditionDto
                        {
                            Severity = (int)HttpStatusCode.InternalServerError,
                            ErrorMessage = String.Join(" ", identityUser.Errors.Select(e => e.Description))
                        }
                    }
                };               
                return (validationResult, null);
            }

            var userIdentity = await _userRepository.GetByUserName(criteria.UserName);   
            
            return (new ValidationResultDto(), userIdentity.Map());
        }

        public List<UserDto> Get()
        {
            var users = _userRepository.Get();

            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                var userDto = user.Map();
                userDto = HydrateRoles(userDto, user);
                usersDto.Add(userDto);
            }

            return usersDto;
        }

        public async Task<(ValidationResultDto ValidationResultDto, UserDto User)> GetByEmail(string email)
        {
            var userResult = await _userValidationService.ValidateUserEmail(email);

            if (userResult.validationResultDto.Conditions.Any())
            {
                return (userResult.validationResultDto, null);
            }

            var userDto = userResult.identityUser.Map();
            userDto = HydrateRoles(userDto, userResult.identityUser);

            return (new ValidationResultDto(), userDto);
        }

      
        public async Task<(ValidationResultDto ValidationResultDto, UserDto User)> GetByUserName(string userName)
        {
            var userResult = await _userValidationService.ValidateUser(userName);

            if (userResult.validationResultDto.Conditions.Any())
            {
                return (userResult.validationResultDto, null);
            }

            var userDto = userResult.identityUser.Map();
            userDto = HydrateRoles(userDto, userResult.identityUser);

            return (new ValidationResultDto(), userDto);
        }

        public async Task<UserDto> GetcurrentUser(System.Security.Claims.ClaimsPrincipal user)
        {

            var result = await _userRepository.GetCurrentUserAsync(user);

            var userDto = result.Map();
            userDto = HydrateRoles(userDto, result);

            return userDto;
        }

        public async Task<(ValidationResultDto ValidationResultDto, UserDto User)> AddRole(string userName, string role)
        {
            var userAndRoleResult = await _userValidationService.ValidateUserAndRole(userName, role);

            if (userAndRoleResult.validationResultDto.Conditions.Any())
            {
                return (userAndRoleResult.validationResultDto, null);
            }

            var addRoleResult = await _userRepository.AddRole(userAndRoleResult.identityUser, userAndRoleResult.identityRole.Name);

            if (!addRoleResult.Succeeded)
            {
                var validationResult = new ValidationResultDto
                {
                    Conditions = new List<ValidationConditionDto>
                    {
                        new ValidationConditionDto
                        {
                            Severity = (int)HttpStatusCode.InternalServerError,
                            ErrorMessage = String.Join(" ", addRoleResult.Errors.Select(e => e.Description))
                        }
                    }
                };

                return (validationResult, null);
            }

            var userDto = userAndRoleResult.identityUser.Map();
            userDto = HydrateRoles(userDto, userAndRoleResult.identityUser);

            return (new ValidationResultDto(), userDto);
        }
        
        public async Task<(ValidationResultDto ValidationResultDto, UserDto User)> RemoveRole(string userName, string role)
        {
            var userAndRoleResult = await _userValidationService.ValidateUserAndRole(userName, role);

            if (userAndRoleResult.validationResultDto.Conditions.Any())
            {
                return (userAndRoleResult.validationResultDto, null);
            }

            var removeRoleResult  = await _userRepository.RemoveRole(userAndRoleResult.identityUser, userAndRoleResult.identityRole.Name);

            if (!removeRoleResult.Succeeded)
            {
                var validationResult = new ValidationResultDto
                {
                    Conditions = new List<ValidationConditionDto>
                    {
                        new ValidationConditionDto
                        {
                            Severity = (int)HttpStatusCode.InternalServerError,
                            ErrorMessage = String.Join(" ", removeRoleResult.Errors.Select(e => e.Description))
                        }
                    }
                };

                return (validationResult, null);
            }

            var userDto = userAndRoleResult.identityUser.Map();
            userDto = HydrateRoles(userDto, userAndRoleResult.identityUser);

            return (new ValidationResultDto(), userDto);

        }

        public async Task<(ValidationResultDto ValidationResultDto, UserDto User)> SignIn(LogInCriteriaDto criteria)
        {
            var validationResult = new ValidationResultDto();

            var identityUserResult = await _userValidationService.ValidateForSignIn(criteria.UserName, criteria.Password);

            if (identityUserResult.validationResultDto.Conditions.Any())
            {
                return (identityUserResult.validationResultDto, null);
            }

            var signIn = await _userRepository.SignIn(identityUserResult.identityUser.UserName, criteria.Password, criteria.RememberMe);

            if (!signIn.Succeeded)
            {
                validationResult.Conditions.Add(
                    new ValidationConditionDto
                    {
                        Severity = (int)HttpStatusCode.Forbidden,
                        ErrorMessage = "Invalid access data"
                    }
                );
                return (validationResult, null);
            }

            var userDto = identityUserResult.identityUser.Map();            
            userDto = HydrateRoles(userDto, identityUserResult.identityUser);

            return (validationResult, userDto);
        }

        public void SignOut()
        {
            _userRepository.SignOut();
        }

        private UserDto HydrateRoles(UserDto userDto, AppUser identityUser)
        {            
            var userRoles = _userRepository.GetUserRoles(identityUser).Result;           

            if (userRoles != null)
            {
                var roles = _roleRepository.Get();

                foreach (var userRol in userRoles)
                {
                    var rol = roles.FirstOrDefault(r => r.Name == userRol);
                    if (rol != null)
                    {
                        //ToDo Mapping
                        userDto.Roles.Add(new RoleDto
                        {
                            Id = rol.Id,
                            Name = rol.Name,
                            NormalizedName = rol.NormalizedName
                        });
                    }                    
                }
            }

            return userDto;
        }



    }
}
