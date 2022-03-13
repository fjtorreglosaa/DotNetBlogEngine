using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Data.Entities
{
    // <summary>
    /// User Entity
    /// </summary>
    public class User
    {
        /// <summary>
        /// The opnly identifier of an User
        /// </summary>
        public string IdentityId { get; set; }       
        public string Email { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public int AuthorId { get; set; }

    }
}
