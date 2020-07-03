using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCode.ViewModels
{
    public class CreateAppRoles:IdentityRole

    {
        public CreateAppRoles()
        {
            //When we Create Any List in Our Domain Class
            // then We Must Have to Initialize that list in the
            // Constructor of that Domain Class like under this

           Users = new List<string>();
        }
        
        public string UserId { get; set; }

        public string UserName { get; set; }

        public bool IsSelected { get; set; }

        [Display(Name =" Role Id")]
        public string RoleId { get; set; }

        [Required]
        [Display(Name ="Role Name")]
        [Remote(action: "IsRoleExist", controller: "RoleManagment")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
