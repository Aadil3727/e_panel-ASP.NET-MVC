using AdminData.Models.Admin;
using AdminServices.Interfaces.Admin;
using AdminServices.Mapper;
using AutoMapper;
using Demoproject.Models;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Admin.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

namespace AdminServices.Services.Admin
{
    public class AuthServices : IAdmin

    {
        private readonly ApplicationContext _context;
        private readonly FileUpload _fileupload;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public AuthServices(ApplicationContext context, FileUpload fileupload, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _fileupload = fileupload;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            }
        private void SetAuthenticationCookie(Guid userId)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("userid", userId.ToString(), new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict, // Adjust based on your security requirements
                Secure = true // Adjust based on your security requirements (HTTPS)
            });
        }
        public EditUserViewModel GetUserById(Guid userId)
        {
            var user = _context.Auth_user.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                var editUserViewModel = new EditUserViewModel
                {
                    Id = user.Id, // Modify this line based on the actual property name
                    Name = user.Name,
                    Email = user.Email,

                    ProfileImg = user.ProfileImg// Modify this line based on the actual property name


                };

                return editUserViewModel;
            }

            return null; // or throw an exception, depending on your requirements
        }

        public string Login(string Email, string Password)
        {
            var response = "";
            try
            {
                Email = Email.Trim();
                var encryptionpassword = Encryption.Encrypt(Password);
                var user = _context.Auth_user.Where(x => x.Email == Email).FirstOrDefault();

                if (user != null)
                {
                    if (user.Password == encryptionpassword)
                    {
                        SetAuthenticationCookie(user.Id);
                        response = "Login successful";
                    }
                    else
                    {
                        response = "Incorrect password";
                    }
                }
                else
                {
                    response = "User not found";
                }

                return response;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return response;
            }
        }

        public async Task<string> Register(AuthDTO admin_Db, IFormFile ProfileImg)
        {
            var response = "";
            try
            {
                string filePath = await _fileupload.UploadProductFile(ProfileImg);
                admin_Db.ProfileImg = filePath;
                admin_Db.Email = admin_Db.Email.Trim();
                var authUserEntity = _mapper.Map<Admin_db>(admin_Db);


                authUserEntity.Password = Encryption.Encrypt(admin_Db.Password);
                _context.Auth_user.Add(authUserEntity);
                _context.SaveChanges();
                response = "Success";
                return response;
            }
            catch (Exception ex)
            {
                response = "Failed";
                return response;
            }
        }
        public void Logout()
        {
            // Sign out the user
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the "userid" cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("userid");
        }
        public async Task SendPasswordResetEmail(string toEmail, string resetToken)
        {

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("test25943026@gmail.com", "inotbtixswttcxlm"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("test25943026@gmail.com"),
                    Subject = "Password Reset",
                    Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Arial', sans-serif;
                    background-color: #f4f4f4;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    border-radius: 5px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .reset-link {{
                    display: block;
                    padding: 10px;
                    background-color: blue;
                    color: #f4f4f4;
                    text-align: center;
                    text-decoration: none;
                    border-radius: 5px;
                }}

            </style>
        </head>
        <body>
            <div class='container'>
                <p>Hello,</p>
                 <p>Click the following link to reset your password:</p>
                <center><a href='https://localhost:7172/Adminview/ResetPassword?token={resetToken}'><button  class='reset-link'>Reset Password</button></a></center>
                
            </div>
                <p style='Color:#D3D3D3'>This link will expire within 1 hour</p>
        </body>
        </html>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);


                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exception (log, etc.)
                throw new Exception("Failed to send email", ex);
            }
        }
        public async Task<string> ForgotPassword(string email)
        {
            var response = "";
            try
            {
                var user = await _context.Auth_user.SingleOrDefaultAsync(x => x.Email == email);

                if (user != null)
                {
                    // Generate a unique token (you can use Guid.NewGuid() for simplicity)
                    string resetToken = Guid.NewGuid().ToString();

                    // Store the token in the database with a timestamp for expiration
                    user.ResetToken = resetToken;
                    user.ResetTokenExpiration = DateTime.UtcNow.AddSeconds(5); // Set expiration time

                    await _context.SaveChangesAsync();

                    // Send an email with the password reset link
                    await SendPasswordResetEmail(email, resetToken);

                    return response = "Password reset link sent successfully";
                }
                else
                {
                    return response = "User not found";
                }
            }
            catch (Exception ex)
            {
                return "Failed to send password reset link";
            }
        }

        public async Task<string> ResetPassword(string token, string newPassword)
        {
            try
            {
                var user = await _context.Auth_user.SingleOrDefaultAsync(x => x.ResetToken == token);

                if (user != null)
                {
                    if (user.ResetTokenExpiration <= DateTime.UtcNow)
                    {
                        return "Token expired";
                    }

                    user.Password = Encryption.Encrypt(newPassword);
                    user.ResetToken = "-";
                    user.ResetTokenExpiration = null;

                    await _context.SaveChangesAsync();

                    return "Password reset successfully";
                }
                else
                {
                    return "Invalid token or user not found";
                }
            }
            catch (Exception ex)
            {
                return "Failed to reset password";
            }
        }

        public async Task<string> Edit(EditUserViewModel admin_Db, IFormFile ProfileImg)
        {
            var response = "";
            {
                try
                {

                    var data = _context.Auth_user.FirstOrDefault(a => a.Id == admin_Db.Id);

                    if (data != null)
                    {
                        // Hash the password before storing it

                        // Upload new profile image
                        if (ProfileImg != null)
                        {
                            string newImageFileName = await _fileupload.UploadProductFile(ProfileImg);
                            data.ProfileImg = newImageFileName;
                        }
                        data.Name = admin_Db.Name;
                        data.Email = admin_Db.Email;
                        //data.ProfileImg = admin_Db.ProfileImg;
                        data.ResetTokenExpiration = DateTime.UtcNow;

                        _context.Auth_user.Update(data);
                        _context.SaveChanges();
                        response = "Success";
                        return response;
                    }
                    else
                    {
                        response = "Failed";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return response;
                }
            }
        }

      
    }
}
