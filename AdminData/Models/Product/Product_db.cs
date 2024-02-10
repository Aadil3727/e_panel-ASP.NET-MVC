using AdminData.Models.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminData.Models.Product
{
    [Table("product")]

    public class Product_db
    {
        [Key]
        public Guid Id { get; set; }

        public string CategoryId { get; set; }

        public string Adminid { get; set; } 

        [Required(ErrorMessage = "Enter Price")]

        public int? price { get; set; }

        [Required(ErrorMessage = "Enter Name")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Description")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Select Image")]

        public string? ProImg {  get; set; }
        public string? Images { get; set; }

        public Category_db Category { get; set; }
    }
}
