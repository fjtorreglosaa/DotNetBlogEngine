using BlogEngine.Data.Entities;
using BlogEngine.Data.Identity;
using BlogEngine.Dto.Auth;
using Microsoft.AspNetCore.Identity;

namespace BlogEngine.Business.Extensions.Auth
{
    public static class UserExtensions
    {
        public static UserDto Map(this AppUser entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new UserDto
            {
                IdentityId = entity.Id,
                UserName = entity.NormalizedUserName,
                Email = entity.NormalizedEmail,
                AuthorId = entity.AuthorId
            };

            return dto;
        }

        public static User MapToUserEntity(this CreateUserCriteriaDto criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            var entity = new User
            {
                Username = criteria.UserName,
                Password = criteria.Password,
                Email = criteria.Email,
                AuthorId = criteria.AuthorId
            };

            return entity;
        }
    }
}
