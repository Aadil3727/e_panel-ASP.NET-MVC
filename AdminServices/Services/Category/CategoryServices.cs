using AdminData.Models.Admin;
using AdminData.Models.Category;
using AdminData.Models.Product;
using AdminServices.Interfaces.Category;
using AdminServices.Mapper;
using AdminServices.Services.Admin;
using AutoMapper;
using Demoproject.Models;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Admin.Data;

namespace AdminServices.Services.Category
{
    public class CategoryServices : Icategory
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _autoMapperProfile;
        private readonly CateFileUpload _fileUpload;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CategoryServices(ApplicationContext context, IMapper imapper, CateFileUpload fileUpload, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _autoMapperProfile = imapper;
            _fileUpload = fileUpload;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Create(Category_db category_Db, IFormFile CategoryImg)
        {
            var response = "";
            try
            {
                string filePath = await _fileUpload.UploadCategoryFile(CategoryImg);
                category_Db.CategoryImg = filePath;
                var authUserEntity = _autoMapperProfile.Map<Category_db>(category_Db);
                _context.Category.Add(authUserEntity);
                _context.SaveChanges();
                response = "Success";
                return response;
            }
            catch (Exception ex)
            {
                response = "Failed";
                return response;
            }
        }
        public async Task<List<CategoryDTO>> List(string userId)
        {
            var categories = await _context.Category.Where(c => c.Adminid == userId).ToListAsync();
            return _autoMapperProfile.Map<List<CategoryDTO>>(categories);
        }
        public Category_db GetCategoryById(Guid id, string adminid)
        {
            if (id != null)
            {
                var category = _context.Category.Where(p => p.Id == id && p.Adminid == adminid).FirstOrDefault();
                return category;
            }
            else
            {
                // Log or handle the case where the provided id is not a valid Guid
                Console.WriteLine($"Invalid Guid: {id}");
                return null;
            }
        }

        public async Task<string> Edit(CategoryDTO categoryEditModel, IFormFile CategoryImg)
        {
            try
            {
                var categoryDb = await _context.Category.FindAsync(categoryEditModel.Id);

                if (CategoryImg != null)
                {
                    // Handle image upload and update logic
                    string filePath = await _fileUpload.UploadCategoryFile(CategoryImg);
                    categoryDb.CategoryImg = filePath;
                }
                categoryDb.Name = categoryEditModel.Name;
                _context.Category.Update(categoryDb);
               

                _context.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }
        public async Task<string> Delete(Guid categoryid, string adminId)
        {
            var response = "";

            try
            {
                var existingProduct = GetCategoryById(categoryid, adminId);

                if (existingProduct != null)
                {
                    _context.Category.Remove(existingProduct);
                    await _context.SaveChangesAsync();

                    response = "Success";
                }
                else
                {
                    Console.WriteLine($"Product not found for id: {categoryid} and adminid: {adminId}");
                    response = "Failed";
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                response = "Failed";
                return response;
            }
        }

       
    }
}
