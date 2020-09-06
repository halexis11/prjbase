using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace prjbase.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Access { get; set; }

    }
}