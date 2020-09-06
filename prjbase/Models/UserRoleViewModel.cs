using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjbase.Models
{
    public class UserRoleViewModel: IdentityUserRole<string>
    {
        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}