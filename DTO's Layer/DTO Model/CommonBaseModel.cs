using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model
{
    public class CommonBaseModel
    {
        public EditUserViewModel EditUserViewModel { get; set; }
        public IEnumerable<CategoryDTO> CategoryDTOs { get; set; }
    }

}
