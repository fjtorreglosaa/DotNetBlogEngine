using BlogEngine.Dto.Auth;
using Microsoft.AspNetCore.Identity;

namespace BlogEngine.Business.Extensions.Auth
{
    public static class RoleExtensions
    {
        public static RoleDto Map(this IdentityRole entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                NormalizedName = entity.NormalizedName                
            };

            return dto;
        }
    }
}
