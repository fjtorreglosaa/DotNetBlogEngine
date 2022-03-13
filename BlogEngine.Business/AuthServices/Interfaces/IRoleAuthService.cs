using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Business.AuthServices.Contracts
{
    public interface IRoleAuthService
    {
        Task<(ValidationResultDto ValidationResultDto, RoleDto Role)> Create(string rol);
        List<RoleDto> Get();
        Task<(ValidationResultDto ValidationResultDto, RoleDto Role)> GetByName(string roleName);      
    }
}
