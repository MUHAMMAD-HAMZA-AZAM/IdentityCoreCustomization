using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCode.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,User")]
    public class UserManagement : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppCustomer> userManager;
        public UserManagement(RoleManager<IdentityRole> roleManager,
            UserManager<AppCustomer> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult UsersList()
        {
            var users = userManager.Users;

            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(AppCustomer appCustomer)
        {
            return View();
        }

    }
}
