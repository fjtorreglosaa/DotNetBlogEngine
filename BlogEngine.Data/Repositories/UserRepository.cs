using BlogEngine.Data.Entities;
using BlogEngine.Data.Identity;
using BlogEngine.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Create(User user)
        {
            var identityUser = new AppUser
            {
                Email = user.Email,
                UserName = user.Username,
                AuthorId = user.AuthorId

            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result;
        }

        public List<AppUser> Get()
        {
            return _userManager.Users.ToList();
        }

        public async Task<AppUser> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<AppUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> GetByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IdentityResult> Update(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> Delete(AppUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<SignInResult> SignIn(string userName, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, rememberMe, false);
        }

        public async void SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddRole(AppUser user, string Role)
        {
            return await _userManager.AddToRoleAsync(user, Role);
        }

        public async Task<IdentityResult> RemoveRole(AppUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<string>> GetUserRoles(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<AppUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

    }
}
