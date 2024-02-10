using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminServices.Services.Product
{
    public class ProductFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductFileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public async Task<string> UploadProdFile(IFormFile ProImg)
        {
            if (ProImg == null || ProImg.Length == 0)
            {
                return null; // No file or empty file
            }
            

            // Ensure the "uploads" directory exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/ProductImages");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file nameProImg
            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(ProImg.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ProImg.CopyToAsync(fileStream);
            }

            // Return the file path for further use or storage in the database
            return fileName;
        }
    }

}

