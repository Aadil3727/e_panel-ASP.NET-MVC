using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminData.Models.Admin
{
    [Table("Auth_User")]
    public class Admin_db
    {
        [Key]
        public Guid Id { get; set; } 
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }        
        public string? ProfileImg { get; set; }

        public string? ResetToken { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ResetTokenExpiration { get; set; } = DateTime.UtcNow;


    }
}
