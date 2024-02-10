using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminData.Models.Category
{
    [Table("Category")]
    public class Category_db
    {
        [Key]
        public Guid Id { get; set; }
        public string Adminid {get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        public string? CategoryImg { get; set; }
    }
}
