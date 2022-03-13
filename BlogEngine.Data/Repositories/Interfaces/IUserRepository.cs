using BlogEngine.Data.Entities;
using BlogEngine.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Crear un nuevo usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> Create(User user);

        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>
        /// <returns></returns>
        List<AppUser> Get();

        /// <summary>
        /// Buscar por Email
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<AppUser> GetByEmail(string email);
       
        /// <summary>
        /// Buscar por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppUser> GetById(string id);

        /// <summary>
        /// Buscar por UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<AppUser> GetByUserName(string userName);

        /// <summary>
        /// Actualizar Usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> Update(AppUser user);

        /// <summary>
        /// Eliminar un Usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> Delete(AppUser user);

        /// <summary>
        /// Validar Usuario y Clave
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        Task<SignInResult> SignIn(string userName, string password, bool rememberMe);

        /// <summary>
        /// Sign out
        /// </summary>
        void SignOut();

        /// <summary>
        /// Asignar rol a usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Rol"></param>
        /// <returns></returns>
        Task<IdentityResult> AddRole(AppUser user, string Role);

        /// <summary>
        /// Quitar Rol a Usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Rol"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveRole(AppUser user, string role);

        /// <summary>
        /// Get User Roles
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<string>> GetUserRoles(AppUser user);

        /// <summary>
        /// Get current logged user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<AppUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user);

    }
}
