using BlogEngine.Business.Extensions.Auth;
using BlogEngine.Business.ValidationServices.Contracts;
using BlogEngine.Data.Entities;
using BlogEngine.Data.Identity;
using BlogEngine.Data.Repositories.Interfaces;
using BlogEngine.Data.UnitOfWork;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BlogEngine.Business.ValidationServices
{
    public class UserValidationService : IUserValitadionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserValidationService(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<(ValidationResultDto validationResultDto, User UserEntity)> ValidateForCreate(CreateUserCriteriaDto criteria)
        {
            var validationResult = new ValidationResultDto();

            validationResult = ValidateUserFields(validationResult, criteria);

            if (validationResult.Conditions.Any())
            {
                return (validationResult, null);
            }

            validationResult = await ValidateExistingUser(validationResult, criteria);

            if (validationResult.Conditions.Any())
            {
                return (validationResult, null);
            }

            validationResult = await ValidateIAuthorExists(criteria.AuthorId);

            if (validationResult.Conditions.Any())
            {
                return (validationResult, null);
            }

            return (validationResult, criteria.MapToUserEntity());
        }

        private async Task<ValidationResultDto> ValidateIAuthorExists(int id)
        {
            var validationResult = new ValidationResultDto();

            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "Author field is mandatory for this request"
                });
            }

            return validationResult;
        }

        public async Task<(ValidationResultDto validationResultDto, AppUser identityUser, IdentityRole identityRole)> ValidateUserAndRole(string userName, string role)
        {
            var validationResult = new ValidationResultDto();

            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(role))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "The UserName and/or role must be filled in"
                });
                return (validationResult, null, null);
            }

            var identityUser = await _userRepository.GetByUserName(userName);

            if (identityUser == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.NotFound,
                    ErrorMessage = "User does not exist"
                });
            }

            var identityRole = await _roleRepository.GetByName(role);

            if (identityRole == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.NotFound,
                    ErrorMessage = "Role does not exist"
                });
            }

            return (validationResult, identityUser, identityRole);
        }

        public async Task<(ValidationResultDto validationResultDto, AppUser identityUser)> ValidateUser(string userName)
        {
            var validationResult = new ValidationResultDto();

            if (String.IsNullOrEmpty(userName))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "The UserName must be filled in"
                });
                return (validationResult, null);
            }

            var identityUser = await _userRepository.GetByUserName(userName);

            if (identityUser == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.NotFound,
                    ErrorMessage = "User does not exist"
                });
                return (validationResult, null);
            }

            return (validationResult, identityUser);
        }

        public async Task<(ValidationResultDto validationResultDto, AppUser identityUser)> ValidateUserEmail(string email)
        {
            var validationResult = new ValidationResultDto();

            if (String.IsNullOrEmpty(email))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "The Email must be filled in"
                });
                return (validationResult, null);
            }

            if (!isEmailValid(email))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid Email"
                });
                return (validationResult, null);
            }

            var identityUser = await _userRepository.GetByEmail(email);

            if (identityUser == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.NotFound,
                    ErrorMessage = "User does not exist"
                });
                return (validationResult, null);
            }

            return (validationResult, identityUser);
        }

        public async Task<(ValidationResultDto validationResultDto, AppUser identityUser)> ValidateForSignIn(string userNameOrEmail, string password)
        {
            var validationResult = new ValidationResultDto();

            if (String.IsNullOrEmpty(userNameOrEmail) || String.IsNullOrEmpty(password))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "You must fill in the user or email and password"
                });
                return (validationResult, null);
            }

            AppUser identityUser;

            if (isEmailValid(userNameOrEmail))
            {
                identityUser = await _userRepository.GetByEmail(userNameOrEmail);
            }
            else
            {
                identityUser = await _userRepository.GetByUserName(userNameOrEmail);
            }

            if (identityUser == null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.NotFound,
                    ErrorMessage = "User does not exist"
                });

                return (validationResult, null);
            }

            return (validationResult, identityUser);
        }


        private ValidationResultDto ValidateUserFields(ValidationResultDto validationResult, CreateUserCriteriaDto criteria)
        {
            if (String.IsNullOrEmpty(criteria.UserName))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "You must fill in the UserName"
                });
            }
            else if (criteria.UserName.Length < 4)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "The UserName must be at least 4 characters long."
                });
            }

            if (String.IsNullOrEmpty(criteria.Password))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "You must fill in the password"
                });
            }
            else if (criteria.Password.Length < 4)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "The password must have a minimum of 4 characters and/or must be alphanumeric."
                });
            }

            if (String.IsNullOrEmpty(criteria.Email))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "You must fill in the email"
                });
            }
            else if (!isEmailValid(criteria.Email))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "You must fill in a valid email address"
                });
            }

            return validationResult;
        }

        private async Task<ValidationResultDto> ValidateExistingUser(ValidationResultDto validationResult, CreateUserCriteriaDto criteria)
        {
            var userEntity = await _userRepository.GetByUserName(criteria.UserName);

            if (userEntity != null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    Severity = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = $"User already exists - UserName: {criteria.UserName}"
                });
            }

            return validationResult;
        }

        internal bool isEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
