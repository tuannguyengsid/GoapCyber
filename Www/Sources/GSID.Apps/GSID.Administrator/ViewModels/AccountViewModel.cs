using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GSID.WebApp.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "User name"), Required(ErrorMessage = "User name is required."), StringLength(250, ErrorMessage = "The email must be {1} at least {2} characters long", MinimumLength = 3)]
        public string Username{ get; set; }
        [Display(Name = "Password"), Required(ErrorMessage = "Password is required."), StringLength(250, ErrorMessage = "The password must be {1} at least {2} characters long", MinimumLength = 1)]
        public string Password { get; set; }
        public string returnUrl { get; set; }
        [Display(Name = "Remember me")]
        public bool Rememberme { get; set; }
    }
}