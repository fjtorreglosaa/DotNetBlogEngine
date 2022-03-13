using BlogEngine.Dto.Author;
using System.Collections.Generic;

namespace BlogEngine.Dto.Auth
{
    public class UserDto
    {
        public string IdentityId { get; set; }        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }      
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public int AuthorId { get; set; } 
    }
}
