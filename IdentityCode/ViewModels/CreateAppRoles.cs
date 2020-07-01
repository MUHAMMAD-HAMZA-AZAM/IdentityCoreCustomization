using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCode.ViewModels
{
    public class CreateAppRoles
    {
        [Required]
        public string RoleName { get; set; }

    }
}
