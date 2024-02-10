using AdminData.Models.Admin;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminServices.Interfaces.Admin
{
    public interface IAdmin
    {
        public Task<string> Register(AuthDTO admin_Db,IFormFile ProfileImg);
        public string Login(string Email, string Password);

        public Task<string> Edit(EditUserViewModel admin_Db,IFormFile ProfileImg);


        void Logout();

        Task<string> ForgotPassword(string email);

        Task<string> ResetPassword(string token, string newPassword);
        EditUserViewModel GetUserById(Guid userId);
    }
}
