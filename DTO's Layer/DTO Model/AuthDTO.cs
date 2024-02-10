using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model
{


    public class AuthDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Enter Name")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Select Profile Image")]

        public string? ProfileImg { get; set; }

        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

    }
}
