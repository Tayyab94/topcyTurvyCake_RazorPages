using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace topcyTurvyCake_RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        [Display(Name ="Email Address")]
        public string EmailAddress { set; get; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { set; get; }


        public async Task<IActionResult> OnPost()
        {
            var redirectString = HttpContext.Request.Query["ReturnUrl"].ToString();
            var isValidUser = this.EmailAddress == "admin@gmail.com" && this.Password == "password";

            if(isValidUser==false)
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
               
            }
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, this.EmailAddress),
                };


            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            //var user = new ClaimsPrincipal(
            //        new ClaimsIdentity
            //        {
            //            new [] {new Claim(ClaimTypes.Name,EmailAddress)},
            //               sch
            //        }
            //    );
             

           if(redirectString!=null)
            {
                return LocalRedirect(redirectString);
            }
            return Page();

        }


        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(
    CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index");
        }
    }
}
