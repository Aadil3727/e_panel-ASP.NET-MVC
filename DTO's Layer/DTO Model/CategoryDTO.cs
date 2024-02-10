using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Adminid { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Select Category Image")]
        public string? CategoryImg {  get; set; }

        
    }
}
