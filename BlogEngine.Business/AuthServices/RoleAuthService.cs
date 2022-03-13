using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Business.Extensions.Auth;
using BlogEngine.Data.Repositories.Interfaces;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Business.AuthServices
{
    public class RoleAuthService : IRoleAuthService
    {
        public IRoleRepository _roleRepository;

        public RoleAuthService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<(ValidationResultDto ValidationResultDto, RoleDto Role)> Create(string rol)
        {
            var validationResult = new ValidationResultDto();            

            if (String.IsNullOrEmpty(rol))
            {                
                validationResult.Conditions.Add(                    
                    new ValidationConditionDto
                    {
                        Severity = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Role name cannot be empty"
                    }
                ); 
                return (validationResult, null);
            }

            var existingRole = await _roleRepository.GetByName(rol);

            if (existingRole != null)
            {
                validationResult.Conditions.Add(
                    new ValidationConditionDto
                    {
                        Severity = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "The role already exists"
                    }
                );
                return (validationResult, null);
            }

            var roleResult = await _roleRepository.Create(rol.ToUpper());

            if (!roleResult.Succeeded)
            {
                validationResult.Conditions.Add(
                    new ValidationConditionDto
                    {
                        Severity = (int)HttpStatusCode.InternalServerError,
                        ErrorMessage = String.Join(" ", roleResult.Errors.Select(e => e.Description))
                    }
                );
                return (validationResult, null);
            }

            var roleEntity = await _roleRepository.GetByName(rol.ToUpper());           

            return (validationResult, roleEntity.Map());
        }

        public List<RoleDto> Get()
        {
            var rolesResult = _roleRepository.Get();
            return rolesResult.Select(r => r.Map()).ToList();
        }     
              
        public async Task<(ValidationResultDto ValidationResultDto, RoleDto Role)> GetByName(string roleName)
        {
            var validationResult = new ValidationResultDto();            

            if (String.IsNullOrEmpty(roleName))
            {
                validationResult.Conditions.Add(
                    new ValidationConditionDto
                    {
                        Severity = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Role name cannot be empty"
                    }
                );
                return (validationResult, null);
            }

            var role = await _roleRepository.GetByName(roleName);

            if (role == null)
            {
                validationResult.Conditions.Add(
                    new ValidationConditionDto
                    {
                        Severity = (int)HttpStatusCode.NotFound,
                        ErrorMessage = "The role does not exist"
                    }
                );
                return (validationResult, null);
            }

           
            var rolDto =  role.Map();

            return (validationResult, rolDto);
        }
    }
}
