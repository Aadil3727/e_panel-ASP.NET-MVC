using AdminData.Models.Product;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminServices.Interfaces.Product
{
    public interface IProductService
    {
      
        Task<string> Create(ProductDTO product, IFormFile[] ProImg);

        public List<Product_db> List(string adminId);

        Task<int> GetProductCountByUserId(Guid id, string adminid);

        Task<string> Edit(ProductDTO product, IFormFile[] ProImg);

        Product_db GetProductById(Guid id, string adminid);

        Task<string> Delete(Guid productId, string adminId);

    }
}
