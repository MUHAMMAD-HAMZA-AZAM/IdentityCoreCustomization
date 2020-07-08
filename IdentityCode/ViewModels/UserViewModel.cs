using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCode.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
       
        public string FirstName { get; set; }
      
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
