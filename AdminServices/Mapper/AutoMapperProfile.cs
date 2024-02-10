
//using AdminData.Migrations;
using AdminData.Models.Admin;
using AdminData.Models.Category;
using AdminData.Models.Product;
using AdminServices.Interfaces.Category;
using AutoMapper;
using DTO_s_Layer.DTO_Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminServices.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthDTO,Admin_db>();
            CreateMap<Category_db, CategoryDTO>();
            CreateMap<CategoryDTO, Category_db>();
            CreateMap<EditUserViewModel,Admin_db>();
            CreateMap<Product_db,ProductDTO>().ReverseMap();
            
        }
    }
}
