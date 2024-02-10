using AdminServices.Interfaces.Admin;
using AdminServices.Services.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Admin.Data;

namespace AdminServices.CookieHandler
{
    public class CookieHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieHandler(IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
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
      
    }
}
