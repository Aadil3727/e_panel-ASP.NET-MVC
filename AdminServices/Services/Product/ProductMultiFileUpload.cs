using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdminServices.Services.Product
{
    public class ProductMultiFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductMultiFileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public async Task<string[]> UploadProdFiles(IFormFile[] ProImg)
        {
            if (ProImg == null || ProImg.Length == 0)
            {
                return null; // No files or empty array
            }

            // Ensure the "uploads" directory exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/CategoryImages");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Store the file names in a list
            var fileNames = new List<string>();

            foreach (var image in ProImg)
            {
                // Generate a unique file name
                string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(image.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Add the file name to the list
                fileNames.Add(fileName);
            }

            // Return the array of file paths for further use or storage in the database
            return fileNames.ToArray();
        }
    }
}
