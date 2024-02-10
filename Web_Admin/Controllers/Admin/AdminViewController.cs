using AdminData.Models.Admin;
using AdminServices.Interfaces.Admin;
using AdminServices.Services.Admin;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Web_Admin.Data;
using Microsoft.EntityFrameworkCore;

namespace Web_Admin.Controllers.Admin
{
    public class AdminViewController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly FileUpload _fileUpload;
        private readonly IAdmin _iadmin;

        public AdminViewController(ApplicationContext context, IAdmin iadmin, FileUpload fileUpload)
        {
            _context = context;
            _iadmin = iadmin;
            _fileUpload = fileUpload;
        }
        public IActionResult Index()
        {

            return View();

        }

        public IActionResult Login() { return View(); }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var response = _iadmin.Login(Email, Password);
            if (response == "Login successful")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.message = "Invalid credentials. Please double-check your email and password, and try again.";
                return View();
            }

        }

        public IActionResult Logout()
        {
            _iadmin.Logout();

            // Redirect to the login page or any other desired page after logout
            return RedirectToAction("Login", "AdminView");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AuthDTO admin_Db, IFormFile ProfileImg)
        {
            try
            {
                var response = await _iadmin.Register(admin_Db, ProfileImg); // Await the asynchronous task
                if (response == "Success")
                {
                    return RedirectToAction("Login", "AdminView");
                }
                else
                {
                    ViewBag.message = "Not Valid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _iadmin.ForgotPassword(email);
            if (response == "Password reset link sent successfully")
            {
                return RedirectToAction("ForgotPasswordSuccess", "Adminview");
            }
            ViewBag.Message = response;
            return View();
        }

        public IActionResult ResetPassword(string token)
        {
            // You may want to add additional validation or UI elements here
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string ResetToken, string Password)
        {
            var response = await _iadmin.ResetPassword(ResetToken, Password);


            ViewBag.Message = response;

            if (response == "Password reset successfully")
            {
                // Redirect to login page or any other desired page after successful password reset
                return RedirectToAction("ResetPasswordSuccess", "Adminview");
            }

            return View();
        }

        public IActionResult ForgotPasswordSuccess()
        {

            return View();
        }
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }
        public IActionResult UserProfile()
        {
            var userIdString = GetCookie("userid");

            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var user = _iadmin.GetUserById(userId);

                if (user == null)
                {
                    return RedirectToAction("Error");
                }
                var editUserViewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    ProfileImg = user.ProfileImg// Modify this line based on the actual property name

                    // Map other properties...
                };

                return View(editUserViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public string GetCookie(string userid)
        {
            var cookieKey = userid.ToString();
            var cookieValue = Request.Cookies[cookieKey];
            return cookieValue;
        }
        public IActionResult Edit()
        {
            var userIdString = GetCookie("userid");

            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var user = _iadmin.GetUserById(userId);

                if (user == null)
                {
                    return RedirectToAction("Error");
                }
                var editUserViewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    ProfileImg = user.ProfileImg// Modify this line based on the actual property name

                    // Map other properties...
                };

                return View(editUserViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel admin_Db, IFormFile ProfileImg)
        {


            var response = await _iadmin.Edit(admin_Db, ProfileImg);
            //var AdminId = GetCookie("userid").ToString();
            if (response == "Success")
            {
                return RedirectToAction("Edit", "Adminview");

            }
            else
            {
                return RedirectToAction("Edit", "Adminview");

            }


        }
    }
}
