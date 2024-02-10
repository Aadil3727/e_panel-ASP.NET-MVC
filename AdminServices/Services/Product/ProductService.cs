using AdminData.Models.Admin;
using AdminData.Models.Product;
using AdminServices.Interfaces.Product;
using AdminServices.Mapper;
using AdminServices.Services.Admin;
using AutoMapper;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Admin.Data;

namespace AdminServices.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _context;
        private readonly ProductFileUpload _fileUpload;
        private readonly ProductMultiFileUpload _multiFileUpload;
        private readonly IMapper _mapper;
        public ProductService(ApplicationContext context, ProductFileUpload fileUpload, ProductMultiFileUpload productMultiFileUpload, IMapper imapper)
        {
            _context = context;
            _fileUpload = fileUpload;
            _multiFileUpload = productMultiFileUpload;
            _mapper = imapper;
        }


        public async Task<string> Create(ProductDTO product, IFormFile[] ProImg)

        {
            var response = "";
            try
            {
                var data = _context.Category.Where(a => a.Id == Guid.Parse(product.CategoryId)).FirstOrDefault();
                int count = 0;
                foreach (var item in ProImg)
                {
                    if (count == 0)
                    {
                        var path = await _fileUpload.UploadProdFile(item);
                        product.ProImg += path;
                    }
                    else
                    {
                        var filepath = await _fileUpload.UploadProdFile(item);
                        product.Images += Path.Combine(filepath, ",");
                    }
                    count++;
                }

                var ProEntity = _mapper.Map<Product_db>(product);
                var newData = _context.Product_set.Add(ProEntity);
                newData.Entity.Category = data;
                _context.Product_set.Add(newData.Entity);
                await _context.SaveChangesAsync();  // Use SaveChangesAsync

                response = "Success";
                return response;
            }
            catch
            {
                response = "Failed";
                return response;
            }
        }


        public Product_db GetProductById(Guid id, string adminid)
        {
            if (id != null)
            {
                var product = _context.Product_set.Where(p => p.Id == id && p.Adminid == adminid).FirstOrDefault();
                return product;
            }
            else
            {
                // Log or handle the case where the provided id is not a valid Guid
                Console.WriteLine($"Invalid Guid: {id}");
                return null;
            }
        }
        public async Task<string> Edit(ProductDTO product, IFormFile[] ProImg)
        {
            var response = "";
            try
            {
                var data = _context.Category.FirstOrDefault(a => a.Id == Guid.Parse(product.CategoryId));

                if (ProImg != null && ProImg.Length > 0)
                {
                    // New images are selected
                    foreach (var item in ProImg)
                    {
                        var path = await _fileUpload.UploadProdFile(item);

                        if (string.IsNullOrEmpty(product.ProImg))
                        {
                            product.ProImg = path;
                        }
                        else
                        {
                            product.Images += Path.Combine(path, ",");
                        }
                    }
                }

                var existingProduct = GetProductById(product.Id, product.Adminid);

                if (existingProduct != null)
                {
                    // Retain existing image paths if no new images are selected
                    if (ProImg == null || ProImg.Length == 0)
                    {
                        product.ProImg = existingProduct.ProImg;
                        product.Images = existingProduct.Images;
                    }

                    _mapper.Map(product, existingProduct);
                    existingProduct.Category = data;

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();

                    response = "Success";
                }
                else
                {
                    Console.WriteLine($"Product not found for id: {product.Id} and adminid: {product.Adminid}");
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
        public async Task<string> Delete(Guid productId, string adminId)
        {
            var response = "";

            try
            {
                var existingProduct = GetProductById(productId, adminId);

                if (existingProduct != null)
                {
                    _context.Product_set.Remove(existingProduct);
                    await _context.SaveChangesAsync();

                    response = "Success";
                }
                else
                {
                    Console.WriteLine($"Product not found for id: {productId} and adminid: {adminId}");
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






        //public async Task<ProductDTO> GetProductData(string Id, string adminId)
        //{
        //    if (!Guid.TryParse(Id, out var productId))
        //    {
        //        return null;
        //    }

        //    var productEntity = _context.Product_set
        //        .FirstOrDefault(c => c.Id == productId && c.Adminid == adminId);

        //    if (productEntity == null)
        //    {
        //        return null;
        //    }

        //    var productDTO = _mapper.Map<ProductDTO>(productEntity);
        //    return productDTO;
        //}


        //public async Task<string> Edit(ProductDTO product, IFormFile[] ProImg)
        //{
        //    var response = "";
        //    try
        //    {
        //        // Find the existing product entity in the database
        //        var existingProduct = await _context.Product_set
        //            .Include(p => p.Category)
        //            .FirstOrDefaultAsync(p => p.Id == product.Id);

        //        if (existingProduct != null)
        //        {
        //            // Use AutoMapper to map the updated properties from product to existingProduct
        //            _mapper.Map(product, existingProduct);

        //            // Update the category if the CategoryId has changed
        //            var newCategoryId = Guid.Parse(product.CategoryId);
        //            if (existingProduct.Category.Id != newCategoryId)
        //            {
        //                var newCategory = await _context.Category.FirstOrDefaultAsync(c => c.Id == newCategoryId);
        //                existingProduct.Category = newCategory;
        //            }

        //            // Handle product images
        //            int count = 0;
        //            foreach (var item in ProImg)
        //            {
        //                if (count == 0)
        //                {
        //                    var path = await _fileUpload.UploadProdFile(item);
        //                    existingProduct.ProImg = path;
        //                }
        //                else
        //                {
        //                    var filepath = await _fileUpload.UploadProdFile(item);
        //                    existingProduct.Images += Path.Combine(filepath, ",");
        //                }
        //                count++;
        //            }

        //            // Use _context.Update to mark the existingProduct as modified
        //            _context.Update(existingProduct);

        //            // Save changes to the database
        //            await _context.SaveChangesAsync();

        //            response = "Success";
        //        }
        //        else
        //        {
        //            response = "Product not found";
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        Console.WriteLine(ex);

        //        response = "Failed";
        //        return response;
        //    }
        //}



        public List<Product_db> List(string adminId)
        {
            try
            {
                var products = _context.Product_set
                    .Include(p => p.Category) // Assuming Category is the navigation property in Product_set
                    .Where(x => x.Adminid == adminId)
                    .ToList();
                return products;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                Console.WriteLine($"Error in GetProducts: {ex.Message}");
                return null; // You might want to return an empty list or handle the error differently
            }
        }


    }
}
