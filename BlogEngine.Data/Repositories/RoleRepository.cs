using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  BlogEngine.Data.Repositories.Interfaces;

namespace BlogEngine.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {          
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Create(string role)
        {
            var identityRole = GetIdentityRole(role);

            var resultRole = await _roleManager.CreateAsync(identityRole);

            return resultRole;
        }

        public async Task<IdentityResult> Delete(string role)
        {
            var identityRole = GetIdentityRole(role);

            var resultRole = await _roleManager.DeleteAsync(identityRole);
            return resultRole;
        }

        public List<IdentityRole> Get()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IdentityRole> GetByName(string role)
        {
            var resultRole = await _roleManager.FindByNameAsync(role);
            return resultRole;
        }

        public async Task<IdentityResult> Update(IdentityRole role)
        {
            var resultRole = await _roleManager.UpdateAsync(role);
            return resultRole;
        }

        private IdentityRole GetIdentityRole(string role)
        {
            return new IdentityRole
            {
                Name = role
            };
        }
    }
}
