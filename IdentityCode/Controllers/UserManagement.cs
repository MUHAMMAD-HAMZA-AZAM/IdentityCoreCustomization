using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityCode.Models;
using IdentityCode.ViewModels;
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
        public async Task<IActionResult> EditUser(AppCustomer appCustomer)
        {
            var user = await userManager.FindByIdAsync(appCustomer.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $" User not found ";
                return View("Not Found");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id=user.Id,
                FirstName = user.FirstName,
                LastName=user.LastName,
                Email=user.Email,
                Roles=userRoles
            };

                return View(model);
            
           
        }

        [HttpPost]
        public  async Task<IActionResult> EditUser(UserViewModel userViewModel)
        {
            var user = await userManager.FindByIdAsync(userViewModel.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $" User not found ";
                return View("Not Found");
            }
            return View(user);
        }

    }
}
