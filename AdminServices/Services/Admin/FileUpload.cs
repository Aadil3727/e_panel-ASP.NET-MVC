using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace AdminServices.Services.Admin
{

    public class FileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public async Task<string> UploadProductFile(IFormFile ProfileImg)
        {
            if (ProfileImg == null || ProfileImg.Length == 0)
            {
                return null; // No file or empty file
            }

            // Ensure the "uploads" directory exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            string fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(ProfileImg.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImg.CopyToAsync(fileStream);
            }

            // Return the file path for further use or storage in the database
            return fileName;
        }
    }

}
