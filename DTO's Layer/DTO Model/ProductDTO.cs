using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model
{
    [Table("Product")]

    public class ProductDTO
    {
       
        public Guid Id { get; set; }

        public string Adminid { get; set; }

        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Enter Price")]

        public int? price { get; set; }

        [Required(ErrorMessage = "Enter Name")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Description")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Select Image")]

        public string? ProImg { get; set; }
        public string? Images { get; set; }

    }


}

