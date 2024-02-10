using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminServices.Services.Category
{
    public class CateFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CateFileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public async Task<string> UploadCategoryFile(IFormFile CategoryImg)
        {
            if (CategoryImg == null || CategoryImg.Length == 0)
            {
                return null; // No file or empty file
            }

            // Ensure the "uploads" directory exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/CategoryImages");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(CategoryImg.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await CategoryImg.CopyToAsync(fileStream);
            }

            // Return the file path for further use or storage in the database
            return fileName;
        }
    }

}

