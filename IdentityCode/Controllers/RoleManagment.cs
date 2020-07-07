using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using IdentityCode.Models;
using IdentityCode.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCode.Controllers
{
    [Authorize(Roles ="SuperAdmin,Admin,User")]
    public class RoleManagment : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppCustomer> userManager;
        public RoleManagment(RoleManager<IdentityRole> roleManager,
            UserManager<AppCustomer> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


      

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsRoleExist(string RoleName)
        {
           var result= await roleManager.FindByNameAsync(RoleName);
            if(result== null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Role Name *{RoleName} is Already Exist");
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> CreateRole(CreateAppRoles model)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
              IdentityResult result= await roleManager.CreateAsync(identityRole);
                if(result.Succeeded)
                {
                    return RedirectToAction("RolesList", "RoleManagment");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RolesList()
        {
            var roles = roleManager.Roles;

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(IdentityRole identityRole)
        {
            var role = await roleManager.FindByIdAsync(identityRole.Id);
         //   var roleName = await roleManager.FindByNameAsync(identityRole.Name);
           
            if(role==null)
            {
                ViewBag.ErrorMessage = $" No Role Found  ";
                return View("Not Found");
            }
            var model = new CreateAppRoles
            {
                RoleId=role.Id,
                RoleName=role.Name
            };

            foreach(var user in userManager.Users)
            {
              if( await userManager.IsInRoleAsync(user, role.Name))
                {
                    
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(CreateAppRoles createAppRoles)
        {
            var role = await roleManager.FindByIdAsync(createAppRoles.Id);
          //  var roleName = await roleManager.FindByNameAsync(createAppRoles.RoleName);

              if (role == null)
            {
                ViewBag.ErrorMessage = $" Role with this ={createAppRoles.RoleName} is Not Found ";
                return View("Not Found");
            }
            else
            {
                role.Name = createAppRoles.RoleName;
               var result= await roleManager.UpdateAsync(role);
                if(result.Succeeded)
                {

                    return RedirectToAction("RolesList");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(createAppRoles);
            }

        }

        // Add or remove User from the Role 

       
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(CreateAppRoles createAppRoles)
        {
            ViewBag.RoleId = createAppRoles.RoleId;
            var role = await roleManager.FindByIdAsync(createAppRoles.RoleId);
            //  var roleName = await roleManager.FindByNameAsync(createAppRoles.RoleName);
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role Name not found ";
            }
            var model = new List<CreateAppRoles>();
            foreach (var user in userManager.Users)
            {
                var userroledata = new CreateAppRoles
                {
                    UserId = user.Id,
                    UserName = user.Email
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userroledata.IsSelected = true;
                }
                else
                {
                    userroledata.IsSelected = false;
                }
                model.Add(userroledata);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<CreateAppRoles> model,string roleId)
        {
            
            var role = await roleManager.FindByIdAsync(roleId);
            //  var roleName = await roleManager.FindByNameAsync(createAppRoles.RoleName);
            if (role == null)
            {
                ViewBag.ErrorMessage = $" Role Name not found ";
                return View("NotFound");
            }
            
            for(int i=0;i<model.Count;i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && ! (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user,role.Name);
                }
                else if(! (model[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if(result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { id = roleId });
        }

    }
}
