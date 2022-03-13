using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Create A Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> Create(string role);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        List<IdentityRole> Get();

        /// <summary>
        /// Get Rol
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityRole> GetByName(string role);

        /// <summary>
        /// Update Rol, If it was not updated returns an empty string
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> Update(IdentityRole role);

        /// <summary>
        /// Delete rol
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> Delete(string role);
    }

}
