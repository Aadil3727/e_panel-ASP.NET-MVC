using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminData.Models.Category;
using AdminData.Models.Product;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AdminServices.Interfaces.Category
{
    public interface Icategory
    {
        Task<string> Create(Category_db category_Db, IFormFile CategoryImg);
        Task<List<CategoryDTO>> List(string userId);
        Task<string> Edit(CategoryDTO categoryEditModel, IFormFile CategoryImg);
        Category_db GetCategoryById(Guid id, string adminid);
        Task<string> Delete(Guid categoryid, string adminId);

    }
}
